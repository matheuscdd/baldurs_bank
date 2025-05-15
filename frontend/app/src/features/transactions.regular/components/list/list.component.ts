import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DatePickerModule } from 'primeng/datepicker';
import { TransactionServiceRegular } from '../../../../core/services/transaction.regular.service';
import { MessageService } from 'primeng/api';
import { TableModule } from 'primeng/table';
import { tTransaction } from '../../../../types/tTransaction';


@Component({
  selector: 'app-list',
  imports: [DatePickerModule, FormsModule, ButtonModule, TableModule],
  providers: [MessageService],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class ListComponent {
  blockBtn = false;
  rangeDates: Date[] | undefined;
  transactions: tTransaction[] = [];

  private readonly messageService = inject(MessageService);
  public readonly transactionService = inject(TransactionServiceRegular);

  onSearch() {
    if (!this.rangeDates || this.rangeDates.filter(Boolean).length !== 2) return;
    this.blockBtn = true;
    this.rangeDates[1].setHours(23, 59, 0, 0); 
    const [startDate, endDate] = this.rangeDates.map(this.formatDate);
    this.transactionService.search(startDate, endDate)
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
