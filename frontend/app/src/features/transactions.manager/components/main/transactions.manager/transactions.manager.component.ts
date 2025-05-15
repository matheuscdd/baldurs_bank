import { Component } from '@angular/core';
import { ListUserComponent } from '../../list.user/list.user.component';

@Component({
  selector: 'app-transactions-manager',
  imports: [ListUserComponent],
  templateUrl: './transactions.manager.component.html',
  styleUrl: './transactions.manager.component.scss'
})
export class TransactionsManagerComponent {

}
