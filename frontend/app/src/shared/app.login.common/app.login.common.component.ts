import { Component, Input } from '@angular/core';
import { LoginComponent  } from '../../features/login/components/main/login.component';
import { AppTopbarComponent } from '../app.topbar/app.topbar.component';

@Component({
  standalone: true,
  selector: 'app-login-common',
  imports: [LoginComponent, AppTopbarComponent],
  templateUrl: './app.login.common.component.html',
  styleUrl: './app.login.common.component.scss'
})
export class AppLoginCommon {
  @Input() imgPath!: string;
  @Input() title!: string;
  @Input() isManager!: boolean;
  @Input() destination!: string;
}
