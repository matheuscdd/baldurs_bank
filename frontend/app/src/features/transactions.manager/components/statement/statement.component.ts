import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ButtonModule } from 'primeng/button';
import { DialogModule } from 'primeng/dialog';
import { Toast } from 'primeng/toast';
import { FormsModule } from '@angular/forms';
import { TransactionServiceManager } from '../../../../core/services/transaction.manager.service';
import { tTransaction } from '../../../../types/tTransaction';
import { DatePickerModule } from 'primeng/datepicker';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-statement',
  imports: [DialogModule, Toast, ButtonModule, FormsModule, DatePickerModule, TableModule],
  providers: [MessageService],
  templateUrl: './statement.component.html',
  styleUrl: './statement.component.scss'
})
export class StatementComponent {
  value?: number;
  blockBtn = false;
  rangeDates: Date[] | undefined;
  transactions: tTransaction[] = [];
  @Input() visibleModal = false;
  @Input() accountId = '';
  @Input() accountNumber = 0;
  @Output() hideModal = new EventEmitter<void>();

  private readonly messageService = inject(MessageService);
  public readonly transactionService = inject(TransactionServiceManager);

  onClose() {
    this.rangeDates = [];
    this.transactions = [];
    this.value = undefined;
    this.hideModal.emit();
  }

  onSearch() {
    if (!this.rangeDates || this.rangeDates.length !== 2) return;
    this.blockBtn = true;
    this.rangeDates[1].setHours(23, 59, 0, 0); 
    const [startDate, endDate] = this.rangeDates.map(this.formatDate);
    this.transactionService.search(this.accountId, startDate, endDate)
    .subscribe({
        next: (response) => {
          this.blockBtn = false;
          response.reverse();
          this.transactions = response;
        },
        error: ({error}) => {
          this.blockBtn = false;
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error?.title || 'Oops... something went wrong', life: 5000 });
        }
      });
  }

  private formatDate(date: Date): string {
    return new Date(
        date.getTime() - date.getTimezoneOffset() * 60 * 1000,
    ).toISOString();
  }
  }
  