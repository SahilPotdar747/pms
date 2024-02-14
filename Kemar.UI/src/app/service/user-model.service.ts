import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export class UserLogin {
  username!: string;
  password!: string;
}

export class UserRegistration {
  id!: number;
  userName!: string;
  firstName!: string;
  lastName!: string;
  email!: string;
  password!: string;
  confirmPassword!: string;
  acceptTerms: boolean = true;
  departmentId!: number;
  designationId!: number;
  role!: string;
  roleId!: number;
  name!: string;
  Created!: string;
  mobileNumber!: string;
  reportingUser!: number;
  dateOfBirth!:Date;
  empCode!:string;
}

// export class DateValidators {
//   static greaterThan(startControl: AbstractControl): ValidatorFn {
//     return (endControl: AbstractControl): ValidationErrors | null => {
//       const startDate: Date = startControl.value;
//       const endDate: Date = endControl.value;
//       if (!startDate || !endDate) {
//         return null;
//       }
//       if (startDate >= endDate) {
//         return { greaterThan: true };
//       }
//       return null;
//     };
//   }
// }

// export class DateValidators {
//   static dateLessThan(dateField1: string, dateField2: string, validatorField: { [key: string]: boolean }): ValidatorFn {
//       return (c: AbstractControl): { [key: string]: boolean } | null => {
//           const date1 = c.get(dateField1).value;
//           const date2 = c.get(dateField2).value;
//           if ((date1 !== null && date2 !== null) && date1 > date2) {
//               return validatorField;
//           }
//           return null;
//       };
//   }
// }

// export class DepartmentResponse {
//    departmentId!:number;
//    departmentName!:string;
//    isActive:boolean=true;
// }

export class ProjectMasterResponse {
  projectId!: number;
  projectName!: string;
  description!: string;
  remark!: string;
  managerId!: number;
  startDate!: Date;
  endDate!: Date;
  status!: string;
  isActive!: true;
  totalRecord!: number;
}

export class ChangePassword {
  userName!: string;
  password!: string;
  oldPassword!: string;
  confirmPassword!: string;
}

// export class LoginResponse {
//   firstName!: string;
//   lastName!: string;
//   Email!: string;
//   role!: string;
//   jwtToken!: string;
//   refreshToken!: string;
//   mobNo!:string;
// }

export class LoginResponse {
  firstName!: string;
  lastName!: string;
  Email!: string;
  role!: string;
  jwtToken!: string;
  refreshToken!: string;
  mobNo!: string;
  userName!: string;
  menuAccess!: Array<NavItem>;
}

export class ForgotPasswordRequest {
  userName!: string;
  email!: string;
}

export class ForgotPassword {
  userName!: string;
  otp!: string;
  newPassword!: string;
  confirmPassword!: string;

}
export class ResetPasswordRequest {
  token!: string;
  password!: string;
  confirmPassword!: string;
}

export interface NavItem {
  displayName: string;
  disabled?: boolean;
  iconName: string;
  route?: string;
  routingURL?: string;
  children?: NavItem[];
  menuIcon?: string;
}

export class UserRoleAccessMapping {
  roleName!: string;
  roleId!: number;
  isActive!: boolean;
  userAccessManagerResponse!: Array<UserAccessManagerResponse>
}

export class UserAccessManagerResponse {

  userAccessManagerId!: number;
  roleId!: number;
  roleName!: string;
  userScreenId!: number;
  screenName!: string;
  screenCode!: string;
  canCreate!: boolean;
  canUpdate!: boolean;
  canDeactivate!: boolean
  isActive!: boolean
}

export class AllUserRoleAccess {
  roleId: number = 0;
  roleName: string = "";
  userAccess: string = "";
  status!: boolean;
}
