import { Component, inject } from '@angular/core';
import { AccountServiceService } from '../../../../../core/services/account.service.service';
import { MessageService } from 'primeng/api';
import { Toast } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { tAccount } from '../../../types/tAccount';

@Component({
  selector: 'app-active-account',
  imports: [Toast, ButtonModule],
  providers: [AccountServiceService, MessageService],
  templateUrl: './active.account.component.html',
  styleUrl: './active.account.component.scss'
})
export class ActiveAccountComponent {
  blockBtn = false;

  private readonly messageService = inject(MessageService);
  private readonly accountService = inject(AccountServiceService);

  onActive() {
    this.blockBtn = true;
    this.accountService.active().subscribe({
      next: (response: tAccount) => {
        localStorage.setItem('account', response.Id)
        this.blockBtn = false;
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Account created successfully', life: 5000 });
      },
      error: ({error}) => {
        this.blockBtn = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: error?.title || 'Oops... something went wrong', life: 5000 });
      }
    });
  }

  onInactive() {
    this.blockBtn = true;
    this.accountService.inactive().subscribe({
      next: () => {
        localStorage.removeItem('account');
        this.blockBtn = false;
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Account removed', life: 5000 });
      },
      error: ({error}) => {
        this.blockBtn = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: error?.title || 'Oops... something went wrong', life: 5000 });
      }
    });
  }
}
