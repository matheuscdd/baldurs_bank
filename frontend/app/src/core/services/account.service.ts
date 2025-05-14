import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { tAccount } from '../../features/active.account/types/tAccount';
import { tUser } from '../../features/transactions.regular/types/tUser';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private readonly http = inject(HttpClient);

  active(): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/accounts/create`;
    return this.http.post(url, null, { headers });
  }

  inactiveRegular(): Observable<any> {
    const token = localStorage.getItem('token');
    const accountId = localStorage.getItem('accountId');
    
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/accounts/regular/remove/id/${accountId}`;
    return this.http.delete(url, { headers });
  }

  findByNumber(accountNumber: number): Observable<tUser> {
    const token = localStorage.getItem('token');
    
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/accounts/find/user/number/${accountNumber}`;
    return this.http.get<tUser>(url, { headers });
  }
}
