import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountServiceService {
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

  inactive(): Observable<any> {
    const token = localStorage.getItem('token');
    const accountId = localStorage.getItem('account');
    
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/accounts/regular/remove/id/${accountId}`;
    return this.http.delete(url, { headers });
  }
}
