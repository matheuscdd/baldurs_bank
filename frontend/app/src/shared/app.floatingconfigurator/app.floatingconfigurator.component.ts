import { Component, computed, inject } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { StyleClassModule } from 'primeng/styleclass';
import { AppConfiguratorComponent } from '../app.configurator/app.configurator.component';
import { LayoutService } from '../../core/services/layout.service';

@Component({
    selector: 'app-floating-configurator',
    imports: [ButtonModule, StyleClassModule, AppConfiguratorComponent],
    templateUrl: './app.floatingconfigurator.component.html',
    styleUrl: './app.floatingconfigurator.component.scss'
})
export class AppFloatingConfiguratorComponent {
    LayoutService = inject(LayoutService);

    isDarkTheme = computed(() => this.LayoutService.layoutConfig().darkTheme);

    toggleDarkMode() {
        this.LayoutService.layoutConfig.update((state) => ({ ...state, darkTheme: !state.darkTheme }));
    }
}
