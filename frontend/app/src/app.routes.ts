import { Routes } from '@angular/router';
import { AppLayout } from './shared/app.layout/app.layout.component';
import { RegisterPage } from './pages/register/register.page';
import { HomePage } from './pages/home/home.page';

export const appRoutes: Routes = [
    { path: '', component: RegisterPage },
    { path: 'register', component: RegisterPage },
    { path: 'home', component: HomePage },
    // { path: '**', redirectTo: '/notfound' }
];
