import { Routes } from '@angular/router';
import { AppLayout } from './app/layout/component/app.layout/app.layout.component';
import { RegisterPage } from './app/pages/register/register.page';

export const appRoutes: Routes = [
    { path: 'register', component: RegisterPage },
    // { path: '**', redirectTo: '/notfound' }
];
