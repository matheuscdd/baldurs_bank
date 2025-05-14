import { Component, inject } from '@angular/core';
import { CreditComponent } from '../../credit/credit.component';
import { TransactionServiceRegular } from '../../../../../core/services/transaction.regular.service';
import { DebitComponent } from "../../debit/debit.component";
import { TransferComponent } from '../../transfer/transfer.component';
import { ListComponent } from '../../list/list.component';

@Component({
  selector: 'app-transactions-regular',
  imports: [CreditComponent, CreditComponent, DebitComponent, TransferComponent, ListComponent],
  providers: [],
  templateUrl: './transactions.regular.component.html',
  styleUrl: './transactions.regular.component.scss'
})
export class TransactionsRegularComponent {

  public readonly transactionService = inject(TransactionServiceRegular);

  ngOnInit() {
    this.transactionService.getBalance().subscribe({});
  }
}
