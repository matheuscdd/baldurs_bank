import { Component, inject } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StyleClassModule } from 'primeng/styleclass';
import { LayoutService } from '../../core/services/layout.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
    selector: 'app-topbar',
    standalone: true,
    imports: [RouterModule, CommonModule, StyleClassModule],
    templateUrl: './app.topbar.component.html',
    styleUrl: './app.topbar.component.scss'
})
export class AppTopbarComponent {
    items!: MenuItem[];
    public readonly layoutService = inject(LayoutService);
    private readonly authService = inject(AuthService);

    onLogout() {
        this.authService.logout();
    }
}
