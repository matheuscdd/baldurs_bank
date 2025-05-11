import { Component } from '@angular/core';
import { LoginComponent } from '../../features/login/components/main/login.component';

@Component({
  standalone: true,
  selector: 'page-login',
  imports: [LoginComponent],
  templateUrl: './login.page.html',
  styleUrl: './login.page.scss'
})
export class LoginPage {

}
