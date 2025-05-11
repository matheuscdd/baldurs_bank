import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Auth, createUserWithEmailAndPassword } from '@angular/fire/auth';
import { from, Observable, switchMap, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { tUserRegister } from '../schemas/register.schema';
import { User } from '@firebase/auth';

@Injectable()
export class RegisterService {
  constructor(
    private readonly auth: Auth,
    private readonly http: HttpClient
  ) { }
  
  register(data: tUserRegister): Observable<string> {
    let firebaseUser: User;

    return from(
      this.http.post(`${environment.apiURL}/api/users/validate`, data, {})
    ).pipe(
      switchMap(() => createUserWithEmailAndPassword(this.auth, data.email, data.password)),
      tap(({ user }) => firebaseUser = user),
      switchMap(() => firebaseUser.getIdToken()),
      switchMap(token => 
        this.http.post(`${environment.apiURL}/api/users/create`, data, {
          headers: { Authorization: `Bearer ${token}` }
        })
      ),
      switchMap(() => from(firebaseUser.getIdToken(true))),
      tap(token => localStorage.setItem('token', token))
    );
  }
}
