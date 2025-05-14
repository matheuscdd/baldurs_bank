import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumber } from 'primeng/inputnumber';
import { Toast } from 'primeng/toast';
import { FormsModule } from '@angular/forms';
import { TransactionServiceManager } from '../../../../core/services/transaction.manager.service';
import { AccountService } from '../../../../core/services/account.service';

@Component({
  selector: 'app-transfer',
  imports: [DialogModule, Toast, ButtonModule, InputNumber, FormsModule],
  providers: [MessageService],
  templateUrl: './transfer.component.html',
  styleUrl: './transfer.component.scss'
})
export class TransferComponent {
  value?: number;
  blockBtn = false;
  destinationAccountId = '';
  destinationAccountNumber?: number;
  userName = '';
  @Input() visibleModal = false;
  @Input() originAccountId = '';
  @Input() originAccountNumber = 0;
  @Output() hideModal = new EventEmitter<void>();

  private readonly messageService = inject(MessageService);
  private readonly transactionService = inject(TransactionServiceManager);
  private readonly accountService = inject(AccountService);

  private setToastAccountNotFound() {
    this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Accout not found', life: 5000 });
  }

  onClose() {
    this.destinationAccountNumber = undefined;
    this.value = undefined;
    this.hideModal.emit();
  }

  onGetUser() {
    if (!this.destinationAccountNumber) return;

    if (this.destinationAccountNumber === this.originAccountNumber) {
      return this.setToastAccountNotFound();
    }

    this.accountService.findByNumber(this.destinationAccountNumber)
    .subscribe({
        next: (response) => {
          this.userName = response.Name;
          this.destinationAccountId = response.AccountId;
        },
        error: ({error}) => {
          this.userName = '';
          this.destinationAccountId = '';
          this.setToastAccountNotFound();
        }
      });
  }

  onTransfer() {
    if (!this.value) {
        return this.messageService.add({ severity: 'warn', summary: 'Error', detail: 'Value cannot be empty', life: 5000 });
    } else if (!this.destinationAccountId) {
      return this.messageService.add({ severity: 'warn', summary: 'Error', detail: 'Account cannot be empty', life: 5000 });
    }
      this.blockBtn = true;

      this.transactionService.transfer(Math.abs(this.value), this.originAccountId, this.destinationAccountId)
      .subscribe({
        next: (response) => {
          this.userName = '';
          this.blockBtn = false;
          this.onClose();
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Deposit made successfully', life: 3000 });
        },
        error: ({error}) => {
          this.userName = '';
          this.blockBtn = false;
          this.onClose();
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error?.title || 'Oops... something went wrong', life: 5000 });
        }
      });
    }
  }
  