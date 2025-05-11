import { inject } from "@angular/core";
import { Auth, onAuthStateChanged } from "@angular/fire/auth";
import { CanActivateFn } from "@angular/router";
import { from } from "rxjs";


export const authGuard: CanActivateFn = () => {
  const auth = inject(Auth);

  return from(new Promise<boolean>((resolve) => {
    onAuthStateChanged(auth, async (user) => {
      if (user) {
        const tokenResult = await user.getIdTokenResult(true);
        resolve(!!tokenResult?.token);
      } else {
        resolve(false);
      }
    });
  }));
};