import { Component } from '@angular/core';
import { RegisterComponent } from '../../features/register/components/main/register.component';
import { AppTopbarComponent } from '../../shared/app.topbar/app.topbar.component';

@Component({
  standalone: true,
  selector: 'page-register',
  imports: [RegisterComponent, AppTopbarComponent],
  templateUrl: './register.page.html',
  styleUrl: './register.page.scss'
})
export class RegisterPage {

}
