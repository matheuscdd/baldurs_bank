import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { regularGuard } from './core/guards/regular.guard';
import { managerGuard } from './core/guards/manager.guard';


export const appRoutes: Routes = [
    { 
        path: '', 
        loadComponent: () => import('./pages/landing/landing.page').then(m => m.LandingPage)
    },
    { 
        path: 'login', 
        loadComponent: () => import('./pages/login/login.page').then(m => m.LoginPage)
    },
    { 
        path: 'register', 
        loadComponent: () => import('./pages/register/register.page').then(m => m.RegisterPage)
    },
    { 
        path: 'home', 
        canActivate: [authGuard, regularGuard],
        loadComponent: () => import('./pages/home/home.page').then(m => m.HomePage)
    },
    { 
        path: 'dashboard', 
        canActivate: [authGuard, managerGuard],
        loadComponent: () => import('./pages/dashboard/dashboard.page').then(m => m.DashboardPage)
    }
    // { path: '**', redirectTo: '/notfound' }
];
