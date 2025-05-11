import { Component } from '@angular/core';
import { AppTopbarComponent } from '../../shared/app.topbar/app.topbar.component';

@Component({
  standalone: true,
  selector: 'app-home',
  imports: [AppTopbarComponent],
  templateUrl: './home.page.html',
  styleUrl: './home.page.scss'
})
export class HomePage {

}
