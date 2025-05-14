import { Component } from '@angular/core';
import { AppLoginCommon } from '../../shared/app.login.common/app.login.common.component';

@Component({
  standalone: true,
  selector: 'page-login-regular',
  imports: [AppLoginCommon],
  templateUrl: './login.regular.page.html',
  styleUrl: './login.regular.page.scss'
})
export class LoginPageRegular {

}
