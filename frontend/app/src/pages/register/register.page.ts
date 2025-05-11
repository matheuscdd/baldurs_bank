import { Component } from '@angular/core';
import { RegisterComponent } from '../../features/register/components/main/register.component';

@Component({
  standalone: true,
  selector: 'page-register',
  imports: [RegisterComponent],
  templateUrl: './register.page.html',
  styleUrl: './register.page.scss'
})
export class RegisterPage {

}
