import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router) { }

  canActivate(): boolean {
    const userString = sessionStorage.getItem('user');
    if (!userString) {

      this.router.navigate(['/login']); // Redirect to login page or any other page
      return false;
    }

    // const user = JSON.parse(userString);
    // if (user.Role.includes("admin")) {
    //   this.router.navigate(['/login']);
    //   return false;
    // }
    return true;
  }

}