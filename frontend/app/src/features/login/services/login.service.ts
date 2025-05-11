import { Injectable } from '@angular/core';
import { Auth, signInWithEmailAndPassword } from '@angular/fire/auth';
import { tUserLogin } from '../schemas/login.schema';
import { from, Observable, switchMap, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  constructor(
    private readonly auth: Auth,
  ) { }

  login(data: tUserLogin): Observable<string> {
    return from(
      signInWithEmailAndPassword(this.auth, data.email, data.password)
    ).pipe(
      switchMap(({ user }) => from(user.getIdToken())),
      tap(token => localStorage.setItem('token', token))
    );
  }
}
