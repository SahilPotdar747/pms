import { Component, OnInit, ViewChild } from '@angular/core';
import { KemarServiceService } from '../../service/kemar-service.service';
import { ServiceUrl } from '../../service/service-url.service';
import { Table } from 'primeng/table';
import { ExcelServiceService } from '../../service/excel-service.service';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MessageService } from 'primeng/api';
import {
  UserRegistration,
  ChangePassword,
} from '../../service/user-model.service';
import { faSmileWink } from '@fortawesome/free-solid-svg-icons';
import { LazyLoadEvent } from 'primeng/api';
import { getLocaleDateFormat } from '@angular/common';



@Component({
  selector: "app-user-registration",
  templateUrl: "./user-registration.component.html",
  styleUrls: ["./user-registration.component.scss"],
})
export class UserRegistrationComponent implements OnInit {
  //public departmentResponse: DepartmentResponse = new DepartmentResponse();
  public userRegistration: UserRegistration = new UserRegistration();
  public user: UserRegistration = new UserRegistration();
  public model: ChangePassword = new ChangePassword();
  showMsg: boolean = false;
  form!: FormGroup;
  changePasswordForm!: FormGroup;
  editUserForm!: FormGroup;
  @ViewChild('dt1') dt1: Table | undefined;
  loading = false;
  errMsg = [];
  submitted = false;
  editUserFormSubmitted = false;
  changePasswordFormSubmitted = false;
  userManager: any;
  allUserManager: any;
  activeUserManager: any;
  department: any;
  designation: any;
  displayChangePassword = false;
  roles!: any;
  filter = '';
  key: string = 'name'; //set default
  reverse: boolean = false;
  location: any;
  display1 = false;
  display = false;
  display2 = false;
  $index = 0;
  userNameForPasswordChange!: string;
  todayDate:string='';

  //sorting
  last: any;
  //public filter : string = '';
  filterUserList: string = '';
  pageSize = 15;
  currentPage = 1;
  totalRecords: any;
  skiprow: number = 0;
  dob: any;
  year: number = -18;
  currentDate: any;
  dateOfBirth: any;

  pagechange(event: any) {
    debugger;
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getUserListWithPagination(1);
  }

  constructor(
    private KemarService: KemarServiceService,
    private fb: FormBuilder,
    private message: MessageService
  ) {
    this.KemarService.isLoggedIn$ = true;
  }

  ngOnInit(): void {
    debugger
    this.todayDate = new Date().toISOString().split('T')[0];
    this.getUserListWithPagination(1);
    //this.searchUserListWithPagination(1);
    // this.getAllUser();
    this.getActiveUsers();
    this.ResetEditUser();
    this.ResetNewUser();
    this.ResetChangePassword();
    this.getAllActiveRoles();
    this.getAllActiveDepartment();
    this.getAllActiveDesignation();
    this.currentDate = new Date();
    this.currentDate.setFullYear(this.currentDate.getFullYear() - 18);
    this.dob =  this.currentDate.toISOString().split('T')[0];

  }

  // Search() {
  //   this.skiprow = 0;
  //   this.currentPage = 1;
  //   this.getUserListWithPagination(1);
  // }

  searchUserListWithPagination(currentPage: any) {
    debugger;
    var query = {
      currentPage: currentPage,
      skipRow: this.skiprow,
      rowSize: this.pageSize,
      searchtext: this.filterUserList,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.getAllUserNew,
      query
    ).subscribe(
      (response) => {
        this.userManager = response;
        if (response?.length > 0) {
          this.totalRecords = response[0].totalRecord;
        } else {
          this.totalRecords = 0;
        }
        this.currentPage = currentPage;
        this.loading = false;
      },
      (r) => {
        this.ShowMessage('error', 'Error', r.error.errorMessage);
        this.loading = false;
      }
    );
  }

  getAllActiveRoles() {
    this.KemarService.get<any>(null, ServiceUrl.getAllActiveRoles).subscribe(
      (response) => {
        this.roles = response;
        // console.log(this.roles);
      },
      (r) => {
        alert(r.error.error);
      }
    );
  }

  getAllActiveDepartment() {
    this.KemarService.get<any>(null, ServiceUrl.getActiveDepartment).subscribe(
      (response) => {
        this.department = response;
        // console.log(this.roles);
      },
      (r) => {
        alert(r.error.error);
      }
    );
  }

  getAllActiveDesignation() {
    this.KemarService.get<any>(null, ServiceUrl.getActiveDesignation).subscribe(
      (response) => {
        this.designation = response;
        // console.log(this.roles);
      },
      (r) => {
        alert(r.error.error);
      }
    );
  }

  loadData(event: LazyLoadEvent) {
    console.log(event);
  }

  // custom validator to check that two fields match
  MustMatch(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];

      if (matchingControl.errors && !matchingControl.errors['mustMatch']) {
        return;
      }

      // set error on matchingControl if validation fails
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ mustMatch: true });
      } else {
        matchingControl.setErrors(null);
      }
    };
  }

  onEditUserFormReset(): void {
    this.editUserFormSubmitted = false;
    this.editUserForm.reset();
  }

  onChangePasswordchangeReset(): void {
    this.changePasswordFormSubmitted = false;
    this.changePasswordForm.reset();
  }

  get f(): { [key: string]: AbstractControl } {
    return this.form.controls;
  }

  get f1(): { [key: string]: AbstractControl } {
    return this.editUserForm.controls;
  }

  get f2(): { [key: string]: AbstractControl } {
    return this.changePasswordForm.controls;
  }

  togglePasswordChange() {
    this.displayChangePassword = !this.displayChangePassword;
    if (this.displayChangePassword == false) {
      this.user.password = 'Dummy@1';
      this.user.confirmPassword = 'Dummy@1';
    } else {
      this.user.password = '';
      this.user.confirmPassword = '';
    }
  }

  getUserRoleDisplayName(rolename: any) {
    // let roleDisplayName = this.roles.find(x => x.roleName == rolename)?.displayName;
    let roleDisplayName = this.roles.find(
      (x: { roleName: any }) => x.roleName == rolename
    )?.displayName;

    return roleDisplayName;
  }

  ResetEditUser() {
    const nonWhitespaceRegExp: RegExp = new RegExp("\\w+(\\s\\w+)*");
    this.editUserForm = this.fb.group(
      {
        firstName: [
          '',
          [
            Validators.required,
            Validators.minLength(2),
            Validators.maxLength(20),
            Validators.pattern('[a-zA-Z-]*'),
            Validators.pattern(nonWhitespaceRegExp)
          ],
        ],
        id: [0],
        acceptTerms: [true],
        changePassword: [false],
        lastName: [
          '',
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(20),
            Validators.pattern('[a-zA-Z-]*'),
            Validators.pattern(nonWhitespaceRegExp)
          ],
        ],
        mobileNumber: [
          '',
          [
            Validators.required,
            Validators.minLength(10),
            Validators.maxLength(10),
            Validators.pattern('[0-9]*'),
            Validators.pattern(nonWhitespaceRegExp)
          ],
        ],
        email: [
          '',
          [
            Validators.required,
            Validators.email,
            Validators.minLength(5),
            Validators.maxLength(50),
          ],
        ],
        roleId: ['', Validators.required],
        userName: [
          '',
          [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(20),
            Validators.pattern('[a-zA-Z-]*'),
            Validators.pattern(nonWhitespaceRegExp)
          ],],
        password: [''],
        departmentId: ['', Validators.required],
        designationId: ['', Validators.required],
        reportingUser: ['', Validators.required],
        dateOfBirth: [''],
        empCode: ['', [Validators.minLength(3), Validators.maxLength(10)]],
        confirmPassword: [''],
        isActive: [true],
      },
      {
        validator: this.MustMatch('password', 'confirmPassword'),
      }
    );
  }

  // ForNewUser

  //   ValidateDOB(userRegistration:any) {
  //     var lblError = document.getElementById("lblError");

  //     //Get the date from the TextBox.
  //     this.editUserForm.controls("txtDate").setValue(userRegistration.dateOfBirth?.split('T')[0]);
  //     var regex = /(((0|1)[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/((19|20)\d\d))$/;

  //     //Check whether valid dd/MM/yyyy Date Format.
  //     if (regex.test(dateString)) {
  //         var parts = dateString.split("/");
  //         var dtDOB = new Date(parts[1] + "/" + parts[0] + "/" + parts[2]);
  //         var dtCurrent = new Date();
  //         lblError.innerHTML = "Eligibility 18 years ONLY."
  //         if (dtCurrent.getFullYear() - dtDOB.getFullYear() < 18) {
  //             return false;
  //         }

  //         if (dtCurrent.getFullYear() - dtDOB.getFullYear() == 18) {

  //             //CD: 11/06/2018 and DB: 15/07/2000. Will turned 18 on 15/07/2018.
  //             if (dtCurrent.getMonth() < dtDOB.getMonth()) {
  //                 return false;
  //             }
  //             if (dtCurrent.getMonth() == dtDOB.getMonth()) {
  //                 //CD: 11/06/2018 and DB: 15/06/2000. Will turned 18 on 15/06/2018.
  //                 if (dtCurrent.getDate() < dtDOB.getDate()) {
  //                     return false;
  //                 }
  //             }
  //         }
  //         lblError.innerHTML = "";
  //         return true;
  //     } else {
  //         lblError.innerHTML = "Enter date in dd/MM/yyyy format ONLY."
  //         return false;
  //     }
  // }

  ResetNewUser() {
    const nonWhitespaceRegExp: RegExp = new RegExp("\\S");
    this.form = this.fb.group(
      {
        id: [0],
        firstName: [
          '',
          [
            Validators.required,
            Validators.minLength(2),
            Validators.maxLength(20),
            Validators.pattern('[a-zA-Z]*'),
            Validators.pattern(nonWhitespaceRegExp)
          ],
        ],
        lastName: [
          '',
          [
            Validators.required,
            Validators.minLength(3),
            Validators.maxLength(20),
            Validators.pattern('[a-zA-Z]*'),
            Validators.pattern(nonWhitespaceRegExp)
          ],
        ],
        mobileNumber: [
          '',
          [
            Validators.required,
            Validators.minLength(10),
            Validators.maxLength(10),
            Validators.pattern('[0-9]*'),
            Validators.pattern(nonWhitespaceRegExp)
          ],
        ],
        email: [
          '',
          [
            Validators.required,
            //Validators.email,
            Validators.minLength(5),
            Validators.maxLength(50),
            Validators.pattern("^[a-zA-Z0-9\\.]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$")
          ],
        ],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(15),
            Validators.pattern(
              '^(?=.*[a-z])(?=.*[A-Z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{5,}$'
            ),
          ],
        ],
        confirmPassword: ['', Validators.required],
        roleId: ['', Validators.required],
        userName: [
          '',
          [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(20),
            Validators.pattern('[a-zA-Z-]*'),
            Validators.pattern(nonWhitespaceRegExp)
          ],
        ],
        acceptTerms: [true],
        departmentId: ['', Validators.required],
        designationId: ['', Validators.required],
        reportingUser: ['', Validators.required],
        dateOfBirth: [''],
        empCode:['',[Validators.required, Validators.minLength(3),Validators.maxLength(10),Validators.pattern('[^-\\s][A-Za-z0-9()*%!_@./#&+:=-\\s]*')]],
        isActive: [true],
      },
      { validator: this.MustMatch('password', 'confirmPassword') }
    );
  }

  // For Password Change
  ResetChangePassword() {
    this.changePasswordForm = this.fb.group(
      {
        userName: [''],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(250),
            Validators.pattern(
              '^(?=.*[a-z])(?=.*[A-Z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{5,}$'
            ),
          ],
        ],
        confirmPassword: ['', Validators.required],
      },
      {
        validator: this.MustMatch('password', 'confirmPassword'),
      }
    );
  }

  getAllUser() {
    this.KemarService.get<UserRegistration>(
      null,
      ServiceUrl.getUsersData
    ).subscribe(
      (response) => {
        this.userManager = response;
        // console.log(this.userManager);
      },
      (r) => {
        alert(r.error.error);
        // console.log(r.error.error);
      }
    );
  }

  getActiveUsers() {
    debugger;
    this.KemarService.get<UserRegistration>(
      null,
      ServiceUrl.GetActiveUsers
    ).subscribe(
      (response) => {
        this.activeUserManager = response;
        // console.log(this.userManager);
      },
      (r) => {
        alert(r.error.error);
        // console.log(r.error.error);
      }
    );
  }

  // getUserListWithPagination(currentPage: any) {
  //   debugger;
  //   this.loading = true;
  //   let query = {
  //     currentPage: currentPage,
  //     skiprow: this.skiprow,
  //     rowSize: this.pageSize,
  //     searchtext: this.filterUserList,
  //   };
  //   this.KemarService.get<any>(
  //     null,
  //     ServiceUrl.getUsersWithPagination,
  //     query
  //   ).subscribe(
  //     (response) => {
  //       this.userManager = response;
  //       if (response.length > 0) {
  //         this.totalRecords = response[0].totalRecord;
  //       } else {
  //         this.totalRecords = 0;
  //       }
  //       this.currentPage = currentPage;
  //       this.loading = false;
  //     },
  //     (r) => {
  //       this.loading = false;
  //       this.KemarService.ShowMessage('error', 'Error', r.error.errorMessage);
  //     }
  //   );
  // }

  getUserListWithPagination(currentPage: any) {
    debugger;
    var query = {
      currentPage: currentPage,
      skipRow: this.skiprow,
      rowSize: this.pageSize,
      searchtext: this.filterUserList,
    };
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.getAllUserNew, query).subscribe(
      (response) => {
        this.userManager = response;
        if (response?.length > 0) {
          this.totalRecords = response[0].totalRecord;
        } else {
          this.totalRecords = 0;
        }
        this.currentPage = currentPage;
        this.loading = false;
      },
      (r) => {
        this.ShowMessage('error', 'Error', r.error.errorMessage);
        this.loading = false;
      }
    );
  }

  ShowMessage(messageType: string, title: string, message: string) {
    this.message.add({
      severity: messageType,
      summary: title,
      detail: message,
    });
  }

  registerUser() {
    debugger;
    this.submitted = true;
    if (this.form.invalid) {
      return;
    }
    // var date = new Date();
    // if(this.dateOfBirth == null){
    //   this.dateOfBirth = "0001-01-01"
    // }
    let body = this.form.value;
    var startDate = this.form.controls['dateOfBirth'].value;
    if (startDate == "") {
      this.form.controls['dateOfBirth'].setValue('01-01-1900');
    }
    this.loading = true;
    //console.log(JSON.stringify(this.form.value, null, 2));
    this.KemarService.postPatch<UserRegistration>(
      ServiceUrl.registerUserManager,
      body
    ).subscribe(
      (data) => {
        this.ShowMessage('success', 'Success', 'User registered successfully');
        this.getUserListWithPagination(1);
        // this.closeModalPopup();
      },
      (r) => {
        let msg = '';
        if (r.error != null && r.error.errors != null) {
          if (r.error.errors.Password != null)
            msg = msg + r.error.errors.Password[0];
          if (r.error.errors.Email != null) msg = msg + r.error.errors.Email[0];
          if (msg == '') msg = 'Error in registering user';
        } else {
          msg = r.error;
        }
        this.ShowMessage('error', 'Warning', msg);
      }
    );
    this.display = false;
    this.display1 = false;
    this.loading = false;
  }

  updateUser() {
    debugger;
    this.editUserFormSubmitted = true;
    if (this.editUserForm.invalid) {
      return;
    }
    this.loading = true;
    let body = this.editUserForm.value;
    this.KemarService.postPatch<UserRegistration>(
      ServiceUrl.updateUserDeatils,
      body
    ).subscribe(
      (data) => {
        this.ShowMessage('success', 'Success', 'User updated successfully');
        this.getUserListWithPagination(1);
      },
      (r) => {
        let msg = '';
        if (r.error != null && r.error.errors != null) {
          if (r.error.errors.Password != null)
            msg = msg + r.error.errors.Password[0];
          if (r.error.errors.Email != null) msg = msg + r.error.errors.Email[0];
          if (msg == '') msg = 'Error in registering user';
        } else {
          msg = r.error;
        }
        this.ShowMessage('error', 'Warning', msg);
        // this.ShowMessage('error', 'Error', r.error.errorMessage);
      }
    );
    this.display1 = false;
    this.display = false;
    this.loading = false;
  }

  onReset(): void {
    this.submitted = false;
    this.ResetNewUser();
  }

  editUser(userRegistration: any) {
    debugger;
    this.submitted = false;
    this.ResetEditUser();
    this.editUserForm.patchValue(userRegistration);
    //this.editUserForm.controls['dateOFBirth'].setValue(this.dob.getFullYear());
    this.editUserForm.controls['dateOfBirth'].setValue(userRegistration.dateOfBirth?.split('T')[0]);
    // this.editUserForm.controls['firstName'].setValue(user.firstName);
    // this.editUserForm.controls['lastName'].setValue(user.lastName);
    // this.editUserForm.controls['id'].setValue(user.id);
    // this.editUserForm.controls['mobileNumber'].setValue(user.mobileNumber);
    // this.editUserForm.controls['email'].setValue(user.email);
    // this.editUserForm.controls['userName'].setValue(user.userName);
    // this.editUserForm.controls['roleId'].setValue(user.roleId);
    // this.displayChangePassword = false;
    // this.editUserForm.controls['changePassword'].setValue(false);
    // // this.editUserForm.controls['acceptTerms'].setValue(true);
    // this.editUserForm.controls['password'].setValue('Dummy@1234');
    // this.editUserForm.controls['confirmPassword'].setValue('Dummy@1234');

    this.display1 = true;
    // this.modalService.open(content, ngbModalOptions);
  }

  open() {
    debugger;
    this.submitted = false;
    this.form.reset();
    this.ResetNewUser();
    //this.form.controls['dateOfBirth'].setValue("0001-01-01");
    this.display = true;
  }

  openChangePassword(user: any) {
    this.changePasswordForm.reset();
    this.submitted = false;
    this.userNameForPasswordChange = user.userName;
    this.display2 = true;
  }

  // Only Integer Numbers
  keyPressNumbers(event: any) {
    var charCode = event.which ? event.which : event.keyCode;
    // Only Numbers 0-9
    if (charCode < 48 || charCode > 57) {
      event.preventDefault();
      return false;
    } else {
      return true;
    }
  }

  // keydown code
  keyDownFunction(event: { keyCode: number }, flag: boolean) {
    if (event.keyCode === 13) {
      if (this.display == true && flag == true) {
        this.registerUser();
      } else if (this.display1 == true && flag == false) {
        this.updateUser();
      } else if (this.display2 == true && flag == false) {
        this.changeUserPassword();
      }
    }
  }

  //Change Password
  changeUserPassword() {
    this.submitted = true;
    if (this.changePasswordForm.invalid) {
      return;
    }
    let changePassword = new ChangePassword();
    changePassword.userName = this.userNameForPasswordChange;
    changePassword.password = this.model.password;
    //tbd : need a new api for this
    changePassword.oldPassword = 'Pass@123';
    changePassword.confirmPassword = this.model.confirmPassword;

    this.KemarService.postPatch<string>(
      ServiceUrl.resetPasswordByAdmin,
      changePassword
    ).subscribe(
      (data) => {
        this.ShowMessage('success', 'Success', 'Password changed successfully');
        // Swal.fire("Password changed succesfully", "" , "success");
      },
      (error) => {
        this.errMsg = error;
        if (error.errors != null && error.errors != undefined)
          this.errMsg = error.errors[0];
        // Swal.fire("Error :" + this.errMsg , "" , "error");
        this.ShowMessage('error', 'Error', 'this.errMsg');
      }
    );
    this.submitted = false;
    this.display2 = false;
    this.model = new ChangePassword();
    this.changePasswordForm.reset();
  }
}
