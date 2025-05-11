import {
    inject,
    Injectable
} from '@angular/core';
import {
    Auth,
    onAuthStateChanged,
    signOut
} from '@angular/fire/auth';
import {
    Router
} from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private readonly auth = inject(Auth);
    private readonly router = inject(Router);

    async logout(): Promise < void > {
        return signOut(this.auth).then(() => {
            localStorage.clear();
            this.router.navigate(['/login']);
        });
    }

    async redirectByRole(): Promise < void > {
        onAuthStateChanged(this.auth, async (user) => {
            if (user) {
                const tokenResult = await user.getIdTokenResult(true);
                if (tokenResult?.token) {
                  this.router.navigate([tokenResult.claims['isManager'] ? '/dashboard' : '/home'])
                  return 
                }
            }
        });
    }
}