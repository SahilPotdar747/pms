import { Injectable } from '@angular/core';

const TOKEN_KEY = 'auth-token';
const Refresh_Token = 'refresh-token';
const USER_NAME = 'user-name';
const USER_KEY = 'auth-user';

@Injectable({
  providedIn: 'root'
})
export class TokenStroageService {

  constructor() { }

  signOut() {
    window.sessionStorage.clear();
  }

  public saveToken(token: string) {
    window.sessionStorage.removeItem(TOKEN_KEY);
    window.sessionStorage.setItem(TOKEN_KEY, token);
  }

  public getToken(): string {
    return sessionStorage.getItem(TOKEN_KEY)!;
  }

  public setRefreshToken(refreshToken: string) {
    window.sessionStorage.removeItem(Refresh_Token);
    window.sessionStorage.setItem(Refresh_Token, refreshToken);
  }

  public saveUser(user: any) {
    window.sessionStorage.removeItem(USER_KEY);
    window.sessionStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  public setUserName(userName: string) {
    window.sessionStorage.removeItem(USER_NAME);
    window.sessionStorage.setItem(USER_NAME, userName);
  }

  public getUser() {
    return JSON.parse(sessionStorage.getItem(USER_KEY)!);
  }

}
