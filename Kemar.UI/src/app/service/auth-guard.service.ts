import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot,Router, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthguardServiceService } from './authguard-service.service';
import { RouterModule, Routes } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private Authguardservice: AuthguardServiceService,
    private router: Router) {}
canActivate(): boolean {
if (!this.Authguardservice.gettoken()) {  // change this condition to check token validity
this.router.navigate(['/login']);
}
return this.Authguardservice.gettoken();
}

}
