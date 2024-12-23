import { Injectable } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { BehaviorSubject, filter } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BreadcrumbService {

  private breadcrumbsSubject = new BehaviorSubject<MenuItem[]>([]); // Comincia con un array vuoto
  breadcrumbs$ = this.breadcrumbsSubject.asObservable(); // Esponiamo il subject come Observable

  constructor(private router: Router, private route: ActivatedRoute) {
    // Ascoltiamo gli eventi di navigazione
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe(() => {
      this.updateBreadcrumbs();
    });
  }

  // Metodo per aggiornare i breadcrumb
  private updateBreadcrumbs() {
    let breadcrumbs: MenuItem[] = [];
    let currentRoute = this.route.root;
    let url = '';

    // Funzione ricorsiva per esplorare i figli
    const exploreRoute = (route: ActivatedRoute, url: string) => {
      // Controlliamo se la rotta ha un breadcrumb nei suoi dati
      if (route.snapshot.data['breadcrumb']) {
        const routeUrl = route.snapshot.url.map(segment => segment.path).join('/');
        const fullUrl = `${url}/${routeUrl}`;

        breadcrumbs.push({
          label: route.snapshot.data['breadcrumb'],
          routerLink: fullUrl
        });
      }

      // Esploriamo i figli se presenti
      if (route.children) {
        for (const child of route.children) {
          exploreRoute(child, url);
        }
      }
    };

    // Iniziamo a esplorare dalla rotta principale
    exploreRoute(currentRoute, url);

    // Aggiorniamo i breadcrumbs
    this.breadcrumbsSubject.next(breadcrumbs);
  }
  
  // private updateBreadcrumbs(route: ActivatedRoute, url: string = '', breadcrumbs: MenuItem[] = []) {
  //   const children: ActivatedRoute[] = route.children;

  //   if (children.length === 0) {
  //     // Quando non ci sono più rotte figlie, aggiorna il modello di breadcrumb
  //     this.items = [
  //       { label: 'Home', icon: 'pi pi-home', routerLink: '/' },  // Aggiungi un breadcrumb "Home"
  //       ...breadcrumbs
  //     ];
  //     return;
  //   }

  //   children.forEach(child => {
  //     const routeURL: string = child.snapshot.url.map(segment => segment.path).join('/');
  //     const params = child.snapshot.params;

  //     if (routeURL !== '') {
  //       url += `/${routeURL}`;
  //       breadcrumbs.push({ label: routeURL, routerLink: [url] });
  //     }

  //     this.updateBreadcrumbs(child, url, breadcrumbs);
  //   });
  // }

}
