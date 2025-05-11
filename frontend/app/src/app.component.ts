import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from './core/services/auth.service';

@Component({
    selector: 'app-root',
    standalone: true,
    imports: [RouterModule],
    templateUrl: './app.component.html',
})
export class AppComponent {
    private readonly authService = inject(AuthService);

    ngOnInit() {
        this.authService.redirectByRole();
    }
}
