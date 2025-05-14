import { Component } from '@angular/core';
import { AppTopbarComponent } from '../../shared/app.topbar/app.topbar.component';
import { TransactionsManagerComponent } from '../../features/transactions.manager/components/main/transactions.manager/transactions.manager.component';

@Component({
  standalone: true,
  selector: 'app-dashboard',
  imports: [AppTopbarComponent, TransactionsManagerComponent],
  templateUrl: './dashboard.page.html',
  styleUrl: './dashboard.page.scss'
})
export class DashboardPage {

}
