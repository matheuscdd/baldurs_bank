import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumber } from 'primeng/inputnumber';
import { Toast } from 'primeng/toast';
import { FormsModule } from '@angular/forms';
import { TransactionServiceManager } from '../../../../core/services/transaction.manager.service';

@Component({
  selector: 'app-credit',
  imports: [DialogModule, Toast, ButtonModule, InputNumber, FormsModule],
  providers: [MessageService],
  templateUrl: './credit.component.html',
  styleUrl: './credit.component.scss'
})
export class CreditComponent {
  value?: number;
  blockBtn = false;
  @Input() visibleModal = false;
  @Input() accountId = '';
  @Input() accountNumber = 0;
  @Output() hideModal = new EventEmitter<void>();

  private readonly messageService = inject(MessageService);
  private readonly transactionService = inject(TransactionServiceManager);

  onClose() {
    this.value = undefined;
    this.hideModal.emit();
  }

  onCredit() {
    if(!this.value) {
        return this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Value cannot be empty', life: 5000 });
    }
      this.blockBtn = true;

      this.transactionService.credit(this.accountId, this.value)
      .subscribe({
        next: () => {
          this.blockBtn = false;
          this.onClose();
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Deposit made successfully', life: 3000 });
        },
        error: ({error}) => {
          this.blockBtn = false;
          this.onClose();
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error?.title || 'Oops... something went wrong', life: 5000 });
        }
      });
    }
  }
  