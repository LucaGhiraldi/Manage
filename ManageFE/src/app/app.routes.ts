import { Routes } from '@angular/router';

import { CategoriaDocumentiComponent, DocumentiComponent, DocumentoComponent } from './M-Documenti';
import { LayoutComponent } from './M-Layout';
import { authGuard } from './auth-guard';
import { LoginComponent, RegisterComponent } from './M-Auth';
import { HomeComponent } from './M-Home';

export const routes: Routes = [
    // Rotta Login come maschera iniziale
    { path: 'Login', component: LoginComponent },
    { path: 'Register', component: RegisterComponent },

    { path: '', component: LayoutComponent,
        canActivate: [authGuard], // Protezione a livello di LayoutComponent
        children: [
            { path: '', redirectTo: '/Home', pathMatch: 'full'},
            { path: 'Home', loadComponent: () => import("./M-Home/home/home.component").then((home) => home.HomeComponent), data: { breadcrumb: 'Home' } },
            { path: 'Documenti', loadComponent: () => import("./M-Documenti/Documenti/documenti/documenti.component").then((documenti) => documenti.DocumentiComponent), data: { breadcrumb: 'Documenti' } },
            { path: 'Documenti/:id', loadComponent: () => import("./M-Documenti/Documento/documento/documento.component").then((documento) => documento.DocumentoComponent), data: { breadcrumb: 'Documenti dettaglio' } },
            { path: 'Categoria', loadComponent: () => import("./M-Documenti/CategoriaDocumento/categoria-documenti/categoria-documenti.component").then((categoria) => categoria.CategoriaDocumentiComponent), data: { breadcrumb: 'Categoria' } },
            { path: 'Settings', loadComponent: () => import("./M-Auth/settings/settings.component").then((settings) => settings.SettingsComponent), data: { breadcrumb: 'Settings' } },
            { path: '**', redirectTo: '/Home', pathMatch: 'full'},
        ] 
    },

    // Wildcard: reindirizza a Login per tutte le rotte non trovate
    { path: '**', redirectTo: '/Login', pathMatch: 'full' },
];
