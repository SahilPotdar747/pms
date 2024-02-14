import { Component, OnInit, ViewChild } from '@angular/core';
import { KemarServiceService } from '../../service/kemar-service.service';
import { ServiceUrl } from '../../service/service-url.service';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MessageService } from 'primeng/api';
import { ChangePassword } from '../../service/user-model.service';
import { DataService } from 'src/app/service/data.service';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  errMsg: string = '';
  userDetails: any;
  model: ChangePassword = new ChangePassword();
  submitted = false;
  changePasswordForm!: FormGroup;
  editUserFormSubmitted = false;
  display: boolean = false;
  profileImageUrl = '';

  constructor(
    private KemarService: KemarServiceService,
    private fb: FormBuilder,
    private message: MessageService,
    private dataApi: DataService,
    private router: Router
  ) {
    this.KemarService.isLoggedIn$ = true;
  }

  ngOnInit(): void {
    this.Reset();
    this.model.confirmPassword = '';
    this.model.oldPassword = '';
    this.model.password = '';
    debugger
    this.userDetails = this.dataApi.getUserDetail();
    let profileImage = this.dataApi.getUserProfileImagePath();
    if (profileImage != null && profileImage != undefined) {
      this.profileImageUrl = 'http:\\\\103.240.90.141:6600\\emp\\'+profileImage ;
    }
    else {
      this.profileImageUrl = 'http:\\\\103.240.90.141:6600\\emp\\NoProfile.jpg';
    }
    // console.log(this.userDetails)
  }

  Reset() {
    this.changePasswordForm = this.fb.group({
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
      confirmPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(15),
        ],
      ],
      oldPassword: [
        '',
        [
          Validators.required,
          Validators.minLength(5),
          Validators.maxLength(15),
        ],
      ],
      userName: [''],
    });
  }

  onSubmit() {
    this.editUserFormSubmitted = true;
    if (this.changePasswordForm.invalid) {
      return;
    }
    if (this.model.password != this.model.confirmPassword) {
      this.ShowMessage(
        'warn',
        'Warning',
        'Password and Confirm Password do not match'
      );
    } else if (this.model.oldPassword == this.model.password) {
      this.ShowMessage(
        'warn',
        'Warning',
        'Old password and new Password should not be Same'
      );
    } else {
      this.changeUserPassword();
    }
  }

  get f1(): { [key: string]: AbstractControl } {
    return this.changePasswordForm.controls;
  }

  changeUserPassword() {
    this.submitted = true;
    let body = this.changePasswordForm.value;
    let err = false;
    let changePassword = new ChangePassword();
    changePassword.userName = this.userDetails.userName;
    changePassword.password = this.model.password;
    changePassword.oldPassword = this.model.oldPassword;
    changePassword.confirmPassword = this.model.confirmPassword;

    this.KemarService.postPatch<string>(
      ServiceUrl.changePassword,
      changePassword
    ).subscribe(
      (data) => {
        this.ShowMessage('success', 'Success', 'Password change succesfully');
        setTimeout(() => {
          window.sessionStorage.clear();
          this.router.navigate(['./login']);
        }, 1000);
      },
      (error) => {
        if (error.error == null) {
          this.ShowMessage('success', 'Success', 'Password change succesfully');
          setTimeout(() => {
            window.sessionStorage.clear();
            this.router.navigate(['./login']);
          }, 1000);
        } else {
          err = true;
          this.errMsg = error;
          this.ShowMessage('error', 'Error', error.error);
        }
      }
    );
    this.display = false;
    this.changePasswordForm.reset();
    this.model = new ChangePassword();
    this.submitted = false;
  }

  open() {
    this.editUserFormSubmitted = false;
    this.changePasswordForm.controls['userName'].setValue(
      this.userDetails.userName
    );
    this.display = true;
  }

  ShowMessage(messageType: string, title: string, message: string) {
    this.message.add({
      severity: messageType,
      summary: title,
      detail: message,
    });
  }

  uploadImage(e: any) {
    var file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    if (file.size > 20971520) {
      alert('File size is large; maximum file size 512000 KB');
      return;
    }

    Swal.fire({
      title: 'Are you sure?',
      text: 'Do you want to upload the image?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, upload it!',
      cancelButtonText: 'No, cancel!',
    }).then((result) => {
      if (result.isConfirmed) {
        this.updateImageProfileImage(file);
      }
    });
  }


  updateImageProfileImage(file: any) {
    var fileExtension =
      file.type.replace('image/', '.') == '.jpeg'
        ? '.jpg'
        : file.type.replace('image/', '.');
    const fileForm = new FormData();
    fileForm.append('image', file, this.userDetails.id+fileExtension);
    this.submitted = true;
    this.KemarService.postPatch<string>(
      ServiceUrl.updateUserImageProfile,
      fileForm
    ).subscribe(
      (data) => {
        this.ShowMessage('success', 'Success', data);
      },
      (error) => {
        this.errMsg = error;
        this.ShowMessage('error', 'Error', error.error);
      }
    );
    this.display = false;
    this.changePasswordForm.reset();
    this.model = new ChangePassword();
    this.submitted = false;
  }
}
