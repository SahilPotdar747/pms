import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { KemarServiceService } from '../../service/kemar-service.service';
import { UserLogin, LoginResponse } from '../../service/user-model.service';
import { ServiceUrl } from '../../service/service-url.service';
import { DataService } from '../../service/data.service';
import { TokenStroageService } from '../../service/token-stroage.service';
import { faKey, faUser } from '@fortawesome/free-solid-svg-icons';
import { MessageService } from 'primeng/api';
import { BehaviorSubject, Observable } from 'rxjs';

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.scss"],
})
export class LoginComponent implements OnInit {
  faUser = faUser;
  faKey = faKey;
  form!: FormGroup;
  public userLogin: UserLogin = new UserLogin();
  public loginInvalid = false;
  private formSubmitAttempt = false;
  private returnUrl!: string;
  isLoggedIn = false;
  isLoginFailed = false;
  errorMessage = '';
  roles: string[] = [];
  submitted = false;
  rememberMe = false;
  isPasswordVisible = false;

  private currentUserSubject!: BehaviorSubject<UserLogin>;
  public currentUser!: Observable<UserLogin>;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private kemarClientService: KemarServiceService,
    private dataService: DataService,
    private tokenStorage: TokenStroageService,
    private formBuilder: FormBuilder,
    private message: MessageService
  ) {
    this.kemarClientService.isLoggedIn$ = false;
  }

  Reset() {
    this.form = this.formBuilder.group({
      username: ['', [Validators.required, Validators.minLength(5)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  ngOnInit(): void {
    window.sessionStorage.clear();
    this.Reset();
    this.rememberMe =
      localStorage.getItem('rememberCurrentUser') == 'true' ? true : false;
    if (this.rememberMe == true) {
      this.currentUserSubject = new BehaviorSubject<UserLogin>(
        JSON.parse(localStorage.getItem('currentUser')!)
      );
      this.form.controls['username'].setValue(
        this.currentUserSubject.value['username']
      );
      this.form.controls['password'].setValue(
        this.currentUserSubject.value['password']
      );
    }
    // if (this.tokenStorage.getToken()) {
    //   this.isLoggedIn = true;
    //   this.roles = this.tokenStorage.getUser().roles;
    // }
  }

  OnLoad() {
    this.userLogin.username = '';
    this.userLogin.password = '';
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  login() {
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
    this.submitted = true;
    var body = this.form.value;
    this.kemarClientService
      .postPatch<LoginResponse>(ServiceUrl.authenticate, body)
      .subscribe(
        (response) => {
          // this.spinner.show();
          if (response.jwtToken !== null) {
            // this.spinner.hide();
            this.dataService.setUserDetail(response);

            this.tokenStorage.saveToken(response.jwtToken);
            this.tokenStorage.saveUser(response);

            this.tokenStorage.setUserName(response.userName);
            this.dataService.setUserMenu(response.menuAccess);
            this.tokenStorage.setRefreshToken(response.refreshToken);

            this.isLoginFailed = false;
            this.isLoggedIn = true;
            this.roles = this.tokenStorage.getUser().roles;
            let currentUserRole = this.dataService.getUserDetail().role;
            this.GetUserNavDesignConfig();
            this.kemarClientService.isLoggedIn$ = true;
            if (currentUserRole == 'Admin' || currentUserRole == 'Management')
              this.router.navigate(['/home']);
            else this.router.navigate(['/home']);
          }

          if (this.rememberMe) {
            localStorage.setItem('currentUser', JSON.stringify(body));
            localStorage.setItem(
              'rememberCurrentUser',
              this.rememberMe ? 'true' : 'false'
            );
          } else {
            localStorage.clear();
          }
          this.submitted = false;
        },
        (r) => {
          var message = r?.error?.message;
          if (r?.error?.message != null) {
            message = r?.error?.message;
          } else if (r?.error != null) {
            if (r?.error?.errors != null) {
              if (r.error?.errors?.Password != null) {
                message = r?.error?.errors?.Password[0];
              }
            }
          }
          this.kemarClientService.ShowMessage('error', 'Error', message);
          console.log(r);
          this.submitted = false;
        }
      );
  }

  onSubmit(): void {
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
  }

  GetUserNavDesignConfig() {
    this.kemarClientService.headerMenu = '1';
    this.kemarClientService
      .get<any>(null, ServiceUrl.GetUserNavDesignConfig)
      .subscribe(
        (response) => {
          if (response == '1') {
            this.kemarClientService.headerMenu = '1';
          } else {
            this.kemarClientService.headerMenu = '0';
          }
        },
        (r) => {
          this.kemarClientService.headerMenu = '1';
        }
      );
  }
}
