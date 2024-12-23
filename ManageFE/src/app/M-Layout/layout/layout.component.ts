import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { MenuItem, MessageService } from 'primeng/api';

import { BreadcrumbModule } from 'primeng/breadcrumb';
import { filter } from 'rxjs';
import { AuthService, BreadcrumbService } from '../../M-Services';
import { MessagesModule } from 'primeng/messages';
import { ToastModule } from 'primeng/toast';
import { CommonModule } from '@angular/common';
import { MenuModule } from 'primeng/menu';
import { ButtonModule } from 'primeng/button';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Menu } from 'primeng/menu'; 

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, FontAwesomeModule, BreadcrumbModule, ToastModule, MessagesModule, MenuModule, ButtonModule],
  providers: [AuthService, BreadcrumbService, MessageService],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent implements OnInit {

  @ViewChild('menu') menu!: Menu;

  breadcrumbs: MenuItem[] = [];
  items: MenuItem[] | undefined;

  constructor(private router: Router, 
              private activatedRoute: ActivatedRoute, 
              private messageService: MessageService,
              private authService: AuthService,
              private breadcrumbService: BreadcrumbService) { }

  ngOnInit(): void {
    console.log("ngOnInit");

    this.items = [
      {
          label: 'Options',
          items: [
              {
                  label: 'Settings',
                  icon: 'pi pi-cog',
                  routerLink: '/Settings'
              },
              {
                  label: 'Export',
                  icon: 'pi pi-upload'
              }
          ]
      }
    ];

    // Ascolta i cambiamenti nei breadcrumbs
    this.breadcrumbService.breadcrumbs$.subscribe(breadcrumbs => {
      this.breadcrumbs = breadcrumbs;
    });

  }

  toggleMenu($ev: Event) 
  { 
      this.menu.toggle($ev);
  } 

  // private createBreadcrumbs(route: ActivatedRoute, url: string = '', breadcrumbs: Array<{ label: string, url: string }> = []): Array<{ label: string, url: string }> {
  //   const children: ActivatedRoute[] = route.children;

  //   if (children.length === 0) {
  //     return breadcrumbs;
  //   }

  //   for (const child of children) {
  //     const routeURL: string = child.snapshot.url.map(segment => segment.path).join('/');
  //     if (routeURL !== '') {
  //       url += `/${routeURL}`;
  //     }

  //     breadcrumbs.push({ label: child.snapshot.data['breadcrumb'], url: url });
  //     return this.createBreadcrumbs(child, url, breadcrumbs);
  //   }

  //   return breadcrumbs;
  // }


  onClickSideBar() {
    console.log("entro");

    const toggle = document.getElementById("header-toggle");
    const nav = document.getElementById("nav-bar");
    const bodypd = document.getElementById("body-pd");
    const headerpd = document.getElementById("header");
    
    console.log("toggle -> ", toggle);
    console.log("nav -> ", nav);
    console.log("bodypd -> ", bodypd);
    console.log("headerpd -> ", headerpd);

    if(toggle && nav && bodypd && headerpd) {
      // show navbar
      nav.classList.toggle('shows');
      // change icon
      toggle.classList.toggle('bx-x');
      // add padding to body
      bodypd.classList.toggle('body-pd');
      // add padding to header
      headerpd.classList.toggle('body-pd');
    }
  }

  logout() {
    this.authService.logout();

    this.router.navigate(['Login']); 

    this.messageService.clear();
    this.messageService.add({ severity: 'success', summary: 'Successo', detail: 'Log out effettuato con successo!' })
  }

}
