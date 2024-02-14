import { Injectable } from '@angular/core';
import { SessionStorageService } from 'angular-web-storage';
import { KemarServiceService } from './kemar-service.service';
import { NavItem } from './user-model.service';

@Injectable({
  providedIn: 'root',
})
export class DataService {
  public isLoading = false;

  constructor(
    public session: SessionStorageService,
    private KemarService: KemarServiceService
  ) {}

  setUserDetail(userDetails: any) {
    this.session.set('LoginResponse', userDetails);
  }

  setDamageDetail(damageDetail: any) {
    this.session.set('DamageResponse', damageDetail);
  }

  getDamageDetail(): any {
    return this.session.get('DamageResponse');
  }

  getUserDetail(): any {
    // console.log(this.session.get('LoginResponse'));
    // var sessiondata = this.session.get('LoginResponse');
    // if (sessiondata.role == 'Management' ) sessiondata.role = 'General';
    // return sessiondata;
    return this.session.get('LoginResponse');
  }

  getUserDisplayName(): any {
    return this.getUserDetail().firstName + ' ' + this.getUserDetail().lastName;
  }

  getUserName(): any {
    return this.getUserDetail().userName;
  }

  getUserRole(): any {
    //return 'admin';
    return this.getUserDetail().roleName;
  }

  getUserDepartment(): any {
    //return 'admin';
    return this.getUserDetail().departmentId;
  }

  getUserDesignation(): any {
    //return 'admin';
    return this.getUserDetail().designationId;
  }

  getApiUrl(): string {
    return this.KemarService.END_POINT;
  }

  setUserMenu(menuData: any) {
    this.session.set('menu', menuData);
  }

  getUserMenu(): Array<NavItem> {
    return this.session.get('menu');
  }

  getUserId(): any {
    return this.getUserDetail().id;
  }

  getUserProfileImagePath(): any {
    return this.getUserDetail().profileImagePath;
  }

  getUserEmpCode(): any {
    return this.getUserDetail().empCode;
  }
}
