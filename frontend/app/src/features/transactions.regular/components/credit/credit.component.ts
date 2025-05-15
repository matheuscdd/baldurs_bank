import { Component, inject } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { InputNumber } from 'primeng/inputnumber';
import { Toast } from 'primeng/toast';
import { TransactionServiceRegular } from '../../../../core/services/transaction.regular.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-credit',
  imports: [DialogModule, Toast, ButtonModule, InputNumber, FormsModule],
  providers: [MessageService],
  templateUrl: './credit.component.html',
  styleUrl: './credit.component.scss'
})
export class CreditComponent {
  value!: number;
  blockBtn = false;
  visibleModal = false;

  private readonly messageService = inject(MessageService);
  private readonly transactionService = inject(TransactionServiceRegular);

  showModal() {      
    if(!localStorage.getItem('accountId')) {
        return this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Active account to continue', life: 5000 });
    }
    this.visibleModal = true;
  }

  onCredit() {

    if(!this.value) {
        return this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Value cannot be empty', life: 5000 });
    }
      this.blockBtn = true;

      this.transactionService.credit(this.value)
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
  