import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable, switchMap, tap } from 'rxjs';
import { tBalance } from '../../features/transactions.regular/types/tBalance';
import { tTransaction } from '../../features/transactions.regular/types/tTransaction';

@Injectable({
  providedIn: 'root'
})
export class TransactionServiceRegular {
  balance = signal(0);
  private readonly http = inject(HttpClient);

  formatCurrency(value: number): string {
    return value.toLocaleString('en-US', {style: 'currency', currency: 'USD'});
  }

  credit(value: number): Observable<tBalance> {
    const token = localStorage.getItem('token');
    const accountId = localStorage.getItem('accountId');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/transactions/regular/credit`;
    return this.http.post(url, { accountId , value }, { headers }).pipe(   
      switchMap(() => this.getBalance()),
    );
  }

  debit(value: number): Observable<tBalance> {
    const token = localStorage.getItem('token');
    const accountId = localStorage.getItem('accountId');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/transactions/regular/debit`;
    return this.http.post(url, { accountId , value }, { headers }).pipe(   
      switchMap(() => this.getBalance()),
    );
  }

  transfer(value: number, destinationAccountId: string): Observable<tBalance> {
    const token = localStorage.getItem('token');
    const originAccountId = localStorage.getItem('accountId');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/transactions/regular/transfer`;
    return this.http.post(url, { 
      originAccountId, 
      destinationAccountId, 
      value: value.toString()
    }, { headers }).pipe(   
      switchMap(() => this.getBalance()),
    );
  }

  getBalance(): Observable<tBalance> {
    const token = localStorage.getItem('token');
    const accountId = localStorage.getItem('accountId');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/transactions/regular/balance/${accountId}`;
    return this.http.get<tBalance>(url, { headers }).pipe(
        tap((response: tBalance) => {
          this.balance.set(response.Balance);
        })
      );
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
    const url = `${environment.apiURL}/transactions/regular/list/period?${params}`;
    return this.http.get<tTransaction[]>(url, { headers });
  }
}
