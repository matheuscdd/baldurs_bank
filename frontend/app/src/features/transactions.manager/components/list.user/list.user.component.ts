import { Component, inject } from '@angular/core';
import { tUser } from '../../../../types/tUser';
import { UserServiceManager } from '../../../../core/services/user.manager.service';
import { MessageService } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { AccountService } from '../../../../core/services/account.service';
import { tAccount } from '../../../../types/tAccount';
import { switchMap, tap } from 'rxjs';
import { DialogModule } from 'primeng/dialog';
import { TransactionServiceManager } from '../../../../core/services/transaction.manager.service';
import { Toast } from 'primeng/toast';
import { CreditComponent } from '../credit/credit.component';
import { DebitComponent } from '../debit/debit.component';
import { TransferComponent } from '../transfer/transfer.component';

@Component({
  selector: 'app-list-user',
  imports: [Toast, TableModule, FormsModule, ButtonModule, DialogModule, CreditComponent, DebitComponent, TransferComponent],
  providers: [MessageService,],
  templateUrl: './list.user.component.html',
  styleUrl: './list.user.component.scss'
})
export class ListUserComponent {
  currentAccountId = '';
  currentBalance = 0;
  currentAccountNumber = 0;
  visibleModalTrans = false;
  visibleModalDebit = false;
  visibleModalCred = false;
  visibleModalDel = false;
  visibleModalBalance = false;
  users: tUser[] = [];
  accountsMap: Record<string, tAccount> = {};

  private readonly userService = inject(UserServiceManager);
  private readonly messageService = inject(MessageService);
  private readonly accountService = inject(AccountService);
  public readonly transactionService = inject(TransactionServiceManager);

    confirmDelection() {
        this.accountService.inactiveManager(this.currentAccountId).subscribe({
        next: () => {
            this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Account removed', life: 5000 });
            this.visibleModalDel = false;
            this.renderList();
        },
        error: ({error}) => {
            this.visibleModalDel = false;
            this.messageService.add({
                  severity: 'error',
                  summary: 'Error',
                  detail: error?.title || 'Oops... something went wrong',
                  life: 5000
              });
        }    
    });
    }

  showTrans(accountId?: string, accountNumber?: number) {
    if (!accountId || !accountNumber) return;
    this.currentAccountNumber = accountNumber;
    this.currentAccountId = accountId;
    this.visibleModalTrans = true;
  } 

  hideTrans() {
    this.visibleModalTrans = false;
  }

  showDebit(accountId?: string, accountNumber?: number) {
    if (!accountId || !accountNumber) return;
    this.currentAccountNumber = accountNumber;
    this.currentAccountId = accountId;
    this.visibleModalDebit = true;
  } 

  hideDebit() {
    this.visibleModalDebit = false;
  }

  showCredit(accountId?: string, accountNumber?: number) {
    if (!accountId || !accountNumber) return;
    this.currentAccountNumber = accountNumber;
    this.currentAccountId = accountId;
    this.visibleModalCred = true;
  } 

  hideCredit() {
    this.visibleModalCred = false;
  }

  showDelection(accountId?: string, accountNumber?: number) {
    if (!accountId || !accountNumber) return;
    this.currentAccountNumber = accountNumber;
    this.currentAccountId = accountId;
    this.visibleModalDel = true;
  }  

  showBalance(accountId?: string, accountNumber?: number) {
    if (!accountId || !accountNumber) return;
    this.transactionService.getBalance(accountId).subscribe({
        next: (response) => {
            this.currentBalance = response.Balance;
            this.currentAccountNumber = accountNumber;
            this.visibleModalBalance = true;
        },
        error: ({error}) => {
            console.log(error);
        }    
    });
  }

  renderList() {
    this.users = [];
    this.accountsMap = {};
    this.accountService.list().pipe(
          tap((response) => {
              response.forEach(el => this.accountsMap[el.UserId] = el);
          }),
          switchMap(() => this.userService.list())
      ).subscribe({
          next: (response) => {
              this.users = response;
          },
          error: ({error}) => {
              this.messageService.add({
                  severity: 'error',
                  summary: 'Error',
                  detail: error?.title || 'Oops... something went wrong',
                  life: 5000
              });
          }
      });
  }

  ngOnInit() {
      this.renderList();
  } 
}
