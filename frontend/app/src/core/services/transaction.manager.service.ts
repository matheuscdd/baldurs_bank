import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable, switchMap, tap } from 'rxjs';
import { tBalance } from '../../types/tBalance';
import { tTransaction } from '../../types/tTransaction';

@Injectable({
  providedIn: 'root'
})
export class TransactionServiceManager {
  private readonly http = inject(HttpClient);

  formatCurrency(value: number): string {
    return value.toLocaleString('en-US', {style: 'currency', currency: 'USD'});
  }

  credit(accountId: string, value: number): Observable<tTransaction> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/transactions/manager/credit`;
    return this.http.post<tTransaction>(url, { accountId , value }, { headers });
  }

  debit(accountId: string, value: number): Observable<tTransaction> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/transactions/manager/debit`;
    return this.http.post<tTransaction>(url, { accountId , value }, { headers });
  }

  transfer(value: number, originAccountId: string, destinationAccountId: string): Observable<tTransaction[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/transactions/manager/transfer`;
    return this.http.post<tTransaction[]>(url, { 
      originAccountId, 
      destinationAccountId, 
      value: value.toString()
    }, { headers });
  }

  getBalance(accountId: string): Observable<tBalance> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/transactions/manager/balance/${accountId}`;
    return this.http.get<tBalance>(url, { headers });
  }

  search(startDate: string, endDate: string): Observable<tTransaction[]>
  {
    const token = localStorage.getItem('token');
    const accountId = localStorage.getItem('accountId');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const params = new URLSearchParams({
      AccountId: accountId!,
      StartDate: startDate,
      EndDate: endDate,
    });
    const url = `${environment.apiURL}/transactions/manager/list/period?${params}`;
    return this.http.get<tTransaction[]>(url, { headers });
  }
}
