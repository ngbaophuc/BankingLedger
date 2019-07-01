import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  userProfile = new Subject<{username: string, firstName: string, lastName: string}>();

  constructor(private http: HttpClient) { }

  signIn(signInInfo: {username: string, password: string}) {
    return this.http.post(environment.apiUrl + 'account/signin', signInInfo);
  }

  signUp(signUpInfo: {username: string, firstName: string, lastName: string, password: string}) {
    return this.http.post(environment.apiUrl + 'account/signup', signUpInfo);
  }

  getBalance() {
    return this.http.get(environment.apiUrl + 'account/balance');
  }

  getUserProfile() {
    return this.http.get(environment.apiUrl + 'account/user_profile');
  }

  setToken(token: string) {
    const currentToken = localStorage.getItem('token');

    if (currentToken === token) return;

    localStorage.setItem('token', token);
    this.getUserProfile()
      .subscribe(res => {
        this.userProfile.next(<{username: string, firstName: string, lastName: string}>res);
      }, _ => {
        this.removeToken();
      });
  }

  removeToken() {
    if (!localStorage.getItem('token')) return;

    localStorage.removeItem('token');
    this.userProfile.next(null);
  }
}
