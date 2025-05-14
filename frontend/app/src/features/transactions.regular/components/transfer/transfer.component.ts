import { Component, inject } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumber } from 'primeng/inputnumber';
import { Toast } from 'primeng/toast';
import { TransactionServiceRegular } from '../../../../core/services/transaction.regular.service';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../../../core/services/account.service';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-transfer',
  imports: [DialogModule, Toast, ButtonModule, InputNumber, FormsModule, InputTextModule],
  providers: [MessageService],
  templateUrl: './transfer.component.html',
  styleUrl: './transfer.component.scss'
})
export class TransferComponent {
  accountId: string = String();
  userName: string = String();
  accountNumber?: number;
  value!: number;
  blockBtn = false;
  visibleModal = false;

  private readonly messageService = inject(MessageService);
  public readonly transactionService = inject(TransactionServiceRegular);
  public readonly accountService = inject(AccountService);

  showModal() {      
    this.accountNumber = undefined;
    this.userName = '';
    this.accountId = '';
    if(!localStorage.getItem('accountId')) {
        return this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Active account to continue', life: 5000 });
    }
    this.visibleModal = true;
  }

  private setToastAccountNotFound() {
    this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Accout not found', life: 5000 });
  }

  onGetUser() {
    if (!this.accountNumber) return;

    if (this.accountNumber === Number(localStorage.getItem('accountNumber'))) {
      return this.setToastAccountNotFound();
    }

    this.accountService.findByNumber(this.accountNumber)
    .subscribe({
        next: (response) => {
          this.userName = response.Name;
          this.accountId = response.AccountId;
        },
        error: ({error}) => {
          this.userName = '';
          this.accountId = '';
          this.setToastAccountNotFound();
        }
      });
  }

  onTransfer() {

    if (!this.value) {
        return this.messageService.add({ severity: 'warn', summary: 'Error', detail: 'Value cannot be empty', life: 5000 });
    } else if (!this.accountId) {
      return this.messageService.add({ severity: 'warn', summary: 'Error', detail: 'Account cannot be empty', life: 5000 });
    }

      this.blockBtn = true;

      this.transactionService.transfer(Math.abs(this.value), this.accountId)
      .subscribe({
        next: (response) => {
          this.userName = '';
          this.blockBtn = false;
          this.visibleModal = false;
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Deposit made successfully', life: 3000 });
        },
        error: ({error}) => {
          this.userName = '';
          this.blockBtn = false;
          this.visibleModal = false;
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error?.title || 'Oops... something went wrong', life: 5000 });
        }
      });
    }
  }
  