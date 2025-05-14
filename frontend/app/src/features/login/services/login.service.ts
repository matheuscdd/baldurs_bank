import { inject, Injectable } from '@angular/core';
import { Auth, signInWithEmailAndPassword } from '@angular/fire/auth';
import { tUserLogin } from '../schemas/login.schema';
import { from, Observable, switchMap, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private readonly auth = inject(Auth);

  login(data: tUserLogin, isManager: boolean): Observable<string> {
    return from(
      signInWithEmailAndPassword(this.auth, data.email, data.password)
    ).pipe(
      switchMap(({ user }) => from(user.getIdToken())),
      tap(token => {
        const rawPayload = token.split('.')[1];
        const handlePayload = JSON.parse(atob(rawPayload));
        if (isManager !== handlePayload['isManager']) {
          throw new Error();
        } else {
          localStorage.setItem('token', token);
        }
      })
    );
  }
}
