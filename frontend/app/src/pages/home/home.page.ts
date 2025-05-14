import { Component } from '@angular/core';
import { AppTopbarComponent } from '../../shared/app.topbar/app.topbar.component';
import { TransactionsRegularComponent } from '../../features/transactions.regular/components/main/transactions.regular/transactions.regular.component';

@Component({
  standalone: true,
  selector: 'app-home',
  imports: [AppTopbarComponent, TransactionsRegularComponent],
  templateUrl: './home.page.html',
  styleUrl: './home.page.scss'
})
export class HomePage {

}
