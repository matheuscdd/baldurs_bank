import { Component } from '@angular/core';
import { AppTopbarComponent } from '../../shared/app.topbar/app.topbar.component';

@Component({
  standalone: true,
  selector: 'app-dashboard',
  imports: [AppTopbarComponent],
  templateUrl: './dashboard.page.html',
  styleUrl: './dashboard.page.scss'
})
export class DashboardPage {

}
