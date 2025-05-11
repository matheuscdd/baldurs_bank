import { CanActivateFn } from "@angular/router";

export const regularGuard: CanActivateFn = () => {
    const token = localStorage.getItem('token');
    if (!token) return false;
    const rawPayload = token.split('.')[1];
    const handlePayload = JSON.parse(atob(rawPayload));
    return handlePayload.isManager === false;
}