import { Component } from '@angular/core';
import { AppTopbarComponent } from '../../shared/app.topbar/app.topbar.component';
import { ActiveAccountComponent } from '../../features/active.account/components/main/active.account/active.account.component';

@Component({
  standalone: true,
  selector: 'app-home',
  imports: [AppTopbarComponent, ActiveAccountComponent],
  templateUrl: './home.page.html',
  styleUrl: './home.page.scss'
})
export class HomePage {

}
