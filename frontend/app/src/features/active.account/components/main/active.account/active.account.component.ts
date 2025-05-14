import { Component, inject } from '@angular/core';
import { AccountService } from '../../../../../core/services/account.service';
import { MessageService } from 'primeng/api';
import { Toast } from 'primeng/toast';
import { ButtonModule } from 'primeng/button';
import { tAccount } from '../../../types/tAccount';
import { FormsModule } from '@angular/forms';
import { ToggleButton } from 'primeng/togglebutton';

@Component({
  selector: 'app-active-account',
  imports: [Toast, ButtonModule, FormsModule, ToggleButton],
  providers: [MessageService],
  templateUrl: './active.account.component.html',
  styleUrl: './active.account.component.scss'
})
export class ActiveAccountComponent {
  blockBtn = false;
  status = !!localStorage.getItem('accountId');

  private readonly messageService = inject(MessageService);
  private readonly accountService = inject(AccountService);

  onActive() {
    this.blockBtn = true;
    this.accountService.active().subscribe({
      next: (response: tAccount) => {
        this.status = true;
        localStorage.setItem('accountId', response.Id);
        localStorage.setItem('accountNumber', response.Number.toString());
        this.blockBtn = false;
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Account created successfully', life: 5000 });
      },
      error: ({error}) => {
        this.status = false;
        this.blockBtn = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: error?.title || 'Oops... something went wrong', life: 5000 });
      }
    });
  }

  onInactive() {
    this.blockBtn = true;
    this.accountService.inactiveRegular().subscribe({
      next: () => {
        this.status = false;
        localStorage.removeItem('accountId');
        localStorage.removeItem('accountNumber');
        this.blockBtn = false;
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Account removed', life: 5000 });
      },
      error: ({error}) => {
        this.status = true;
        this.blockBtn = false;
        this.messageService.add({ severity: 'error', summary: 'Error', detail: error?.title || 'Oops... something went wrong', life: 5000 });
      }
    });
  }
}
