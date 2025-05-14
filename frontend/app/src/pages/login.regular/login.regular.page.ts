import { Component } from '@angular/core';
import { LoginRegularComponent } from '../../features/login/components/main/login.component';
import { AppTopbarComponent } from '../../shared/app.topbar/app.topbar.component';

@Component({
  standalone: true,
  selector: 'page-login-regular',
  imports: [LoginRegularComponent, AppTopbarComponent],
  templateUrl: './login.regular.page.html',
  styleUrl: './login.regular.page.scss'
})
export class LoginPage {

}
