import { Component, inject } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumber } from 'primeng/inputnumber';
import { Toast } from 'primeng/toast';
import { TransactionServiceRegular } from '../../../../core/services/transaction.regular.service';
import { FormsModule } from '@angular/forms';
import { tTransaction } from '../../types/tTransaction';

@Component({
  selector: 'app-debit',
  imports: [DialogModule, Toast, ButtonModule, InputNumber, FormsModule],
  providers: [MessageService],
  templateUrl: './debit.component.html',
  styleUrl: './debit.component.scss'
})
export class DebitComponent {
  value!: number;
  blockBtn = false;
  visibleModal = false;

  private readonly messageService = inject(MessageService);
  public readonly transactionService = inject(TransactionServiceRegular);

  showModal() {      
    if(!localStorage.getItem('accountId')) {
        return this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Active account to continue', life: 5000 });
    }
    this.visibleModal = true;
  }

  onDebit() {

    if(!this.value) {
        return this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Value cannot be empty', life: 5000 });
    }
      this.blockBtn = true;

      this.transactionService.debit(Math.abs(this.value) * -1)
      .subscribe({
        next: (response) => {
          this.blockBtn = false;
          this.visibleModal = false;
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Deposit made successfully', life: 3000 });
        },
        error: ({error}) => {
          this.blockBtn = false;
          this.visibleModal = false;
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error?.title || 'Oops... something went wrong', life: 5000 });
        }
      });
    }
  }
  