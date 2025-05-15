import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Observable, switchMap, tap } from 'rxjs';
import { tBalance } from '../../types/tBalance';
import { tTransaction } from '../../types/tTransaction';
import { tUser } from '../../types/tUser';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly http = inject(HttpClient);

  find(userId: string): Observable<tUser> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/users/find/id/${userId}`;
    return this.http.get<tUser>(url, { headers });
  }

  listManager(): Observable<tUser[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/users/manager/list`;
    return this.http.get<tUser[]>(url, { headers });
  }
}
