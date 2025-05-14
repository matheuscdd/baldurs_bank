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
export class UserServiceManager {
  private readonly http = inject(HttpClient);

  list(): Observable<tUser[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    const url = `${environment.apiURL}/users/manager/list`;
    return this.http.get<tUser[]>(url, { headers });
  }
}
