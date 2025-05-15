import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { tUser } from '../../types/tUser';
import { tAccount } from '../../types/tAccount';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private readonly http = inject(HttpClient);

  activeRegular(): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/accounts/regular/create`;
    return this.http.post(url, null, { headers });
  }

  activeManager(userId: string): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/accounts/manager/create/${userId}`;
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

  inactiveManager(accountId: string): Observable<any> {
    const token = localStorage.getItem('token');
    
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/accounts/manager/remove/id/${accountId}`;
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

  findByUser(userId: string): Observable<tAccount> {
    const token = localStorage.getItem('token');
    
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/accounts/find/account/user/${userId}`;
    return this.http.get<tAccount>(url, { headers });
  }

  list(): Observable<tAccount[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/accounts/manager/list`;
    return this.http.get<tAccount[]>(url, { headers });
  }
}
