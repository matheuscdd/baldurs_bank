import { Component, inject } from '@angular/core';
import { CreditComponent } from '../../credit/credit.component';
import { TransactionServiceRegular } from '../../../../../core/services/transaction.regular.service';
import { DebitComponent } from "../../debit/debit.component";
import { TransferComponent } from '../../transfer/transfer.component';
import { ListComponent } from '../../list/list.component';
import { AccountService } from '../../../../../core/services/account.service';
import { switchMap, tap } from 'rxjs';
import { ActiveAccountComponent } from '../../../../active.account/components/main/active.account/active.account.component';

@Component({
  selector: 'app-transactions-regular',
  imports: [ActiveAccountComponent, CreditComponent, CreditComponent, DebitComponent, TransferComponent, ListComponent],
  providers: [],
  templateUrl: './transactions.regular.component.html',
  styleUrl: './transactions.regular.component.scss'
})
export class TransactionsRegularComponent {
  accountNumber = '-';

  public readonly transactionService = inject(TransactionServiceRegular);
  private readonly accountService = inject(AccountService);

  updateAccountNumber(accountNumber?: string) {
    this.accountNumber = accountNumber || '-';
  }

  ngOnInit() {
    const token = localStorage.getItem('token');
    const rawPayload = token!.split('.')[1];
    const handlePayload = JSON.parse(atob(rawPayload));
    this.accountService.findByUser(handlePayload['user_id']).pipe(
    tap(account => {
      localStorage.setItem('accountId', account.Id);
      localStorage.setItem('accountNumber', account.Number.toString());
      this.accountNumber = account.Number.toString();
    }),
  switchMap(() => this.transactionService.getBalance())
  ).subscribe({});
  }
}
