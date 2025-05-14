import { inject, Injectable } from '@angular/core';
import { Auth, signInWithEmailAndPassword, signOut } from '@angular/fire/auth';
import { tUserLogin } from '../schemas/login.schema';
import { from, Observable, switchMap, tap } from 'rxjs';
import { AuthService } from '../../../core/services/auth.service';

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
          localStorage.clear();
          signOut(this.auth);
          throw new Error();
        } else {
          localStorage.setItem('token', token);
        }
      })
    );
  }
}
