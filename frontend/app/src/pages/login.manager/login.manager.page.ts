import { Component } from '@angular/core';
import { AppLoginCommon } from '../../shared/app.login.common/app.login.common.component';

@Component({
  standalone: true,
  selector: 'page-login-manager',
  imports: [AppLoginCommon],
  templateUrl: './login.manager.page.html',
  styleUrl: './login.manager.page.scss'
})
export class LoginPageManager {

}
