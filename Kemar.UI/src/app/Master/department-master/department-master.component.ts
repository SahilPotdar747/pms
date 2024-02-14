import { Component, OnInit, ViewChild } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  NgControl,
  Validators,
} from '@angular/forms';
import { faTruckField } from '@fortawesome/free-solid-svg-icons';
import { ReadingOrder, Table } from 'exceljs';
import { LazyLoadEvent, MessageService } from 'primeng/api';
import { ExcelServiceService } from 'src/app/service/excel-service.service';
import { KemarServiceService } from 'src/app/service/kemar-service.service';
import { ServiceUrl } from 'src/app/service/service-url.service';


@Component({
  selector: 'app-department-master',
  templateUrl: './department-master.component.html',
  styleUrls: ['./department-master.component.scss']
})
export class DepartmentMasterComponent {
 // KemarService: any;
 loading = false;
 @ViewChild('dt') dt: Table | undefined;
 title: string = "'Add Department'";
 deptform!: FormGroup;
 loginUser: string = '';
 submitted!: boolean;
 ShowActive: boolean = false;
 deptMaster: any;
 display: boolean = false;
 Save!: string;
 spinner: boolean = false;
 $index = 0;
 activeUserManager: any;

 //sorting
 public filter: string = '';
 filterDepartmentList: string = '';
 skiprow: number = 0;
 pageSize = 15;
 currentPage: any = 1;
 totalRecords = 0;
 last: any;
 rows: any;
 key!: string;
 reverse: boolean = false;

 constructor(
   private KemarService: KemarServiceService,
   private excelService: ExcelServiceService,
   private fb: FormBuilder,
   private message: MessageService
 ) {
   this.KemarService.isLoggedIn$ = true;
 }

 ngOnInit(): void {
   //this.getAllDepartment();
   this.Reset();
   this.getAllDeptWithPagination(this.currentPage);
   this.getActiveUsers();
   //this.getDeptWithPagination('',0,1);
 }



 Reset() {
   //const nonWhitespaceRegExp: RegExp = new RegExp("\\S");
   this.deptform = this.fb.group({
     departmentId: [0, [Validators.required]],
     departmentName: [
       '',
       [
         Validators.required,
         Validators.minLength(2),
         Validators.maxLength(50),
         //Validators.pattern('[^-\\s][A-Za-z0-9()*%!_@./#&+:=-\\s]*'),
         this.noWhitespaceValidator,
         //Validators.pattern(nonWhitespaceRegExp)
       ],
     ],
     isActive: [true, [Validators.required]],
     coordinatingIncharge: [0],
     coordinatingInchargeName: [''],
   });
 }

 // public noWhitespaceValidator(control: FormControl) {
 //   
 //   const isWhitespace = (control.value || '').trim().length === 0;
 //   const isValid = !isWhitespace;
 //   return isValid ? null : { 'whitespace': true };
 // }

 public noWhitespaceValidator(control: FormControl) {
   let isWhitespace = (control.value || '').trim().length === 0;
   if (!isWhitespace) {
     if (control.value != null || control.value.length > 0) {
       if (control.value.startsWith(' ')) {
         isWhitespace = true;
       } else if (control.value.endsWith(' ')) {
         isWhitespace = true;
       }
     }
   }
   const isValid = !isWhitespace;
   return isValid ? null : { whitespace: true };
 }

 onReset() {
   this.submitted = false;
   this.deptform.reset();
   this.Reset();
 }

 ShowMessage(messageType: string, title: string, message: string) {
   this.message.add({
     severity: messageType,
     summary: title,
     detail: message,
   });
 }

 get f(): { [key: string]: AbstractControl } {
   return this.deptform.controls;
 }

 pagechange(event: any) {
   
   this.skiprow = event.first;
   this.pageSize = event.rows;
   this.getAllDeptWithPagination(this.currentPage);
 }
 getAllDepartment() {
   // this method not using for list
   this.loading = true;
   this.KemarService.get<any>(null, ServiceUrl.getAllDepartment).subscribe(
     (response) => {
       this.deptMaster = response;
     },
     (r) => {
       this.ShowMessage('error', 'Error', r.error.errorMessage);
     }
   );
   this.loading = false;
 }

 // Search() {
 //   this.skiprow = 0;
 //   this.currentPage = 1;
 //   this.searchDeptWithPagination(1);
 // }

 searchDeptWithPagination(currentPage: any) {   //this function using only for keyup type search without loading...
   
   var query = {
     currentPage: currentPage,
     skipRow: this.skiprow,
     rowSize: this.pageSize,
     searchtext: this.filterDepartmentList,
   };
   this.KemarService.get<any>(
     null,
     ServiceUrl.getAllDeptWithPagination,
     query
   ).subscribe(
     (response) => {
       this.deptMaster = response;
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

 getAllDeptWithPagination(currentPage: any) {
   
   var query = {
     currentPage: currentPage,
     skipRow: this.skiprow,
     rowSize: this.pageSize,
     searchtext: this.filterDepartmentList,
   };
   this.loading = true;
   this.KemarService.get<any>(
     null,
     ServiceUrl.getAllDeptWithPagination,
     query
   ).subscribe(
     (response) => {
       this.deptMaster = response;
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

 open() {
   
   this.submitted = false;
   this.deptform.reset();
   this.Reset();
   this.ShowActive = true;
   this.title = 'Add Department';
   this.Save = 'Add';
   // this.deptform.controls['departmentName'].setValue('');
   // this.deptform.controls['isActive'].setValue(true);
   this.display = true;
 }

 editDept(dept: any) {
   this.submitted = false;
   this.ShowActive = true;
   this.deptform.reset();
   this.title = 'Update Department';
   this.deptform.patchValue(dept);
   this.Save = 'Update';
   this.display = true;
 }

 registerDepartment() {
   
   this.submitted = true;
   if (this.deptform.invalid) {
     return;
   }
   this.loading = true;
   let body = this.deptform.value;
   this.KemarService.postPatch<any>(
     ServiceUrl.registerDepartment,
     body
   ).subscribe(
     (res) => {
       this.ShowMessage('success', 'Success', res.responseMessage);
       this.getAllDeptWithPagination(this.currentPage);
     },
     (r) => {
       this.ShowMessage('error', 'Error', r.error.errorMessage);
     }
   );
   this.display = false;
   this.loading = false;
 }

 Inchargeselection() {
   var inchargeId = this.activeUserManager.filter(
     (x: { id: any }) =>
       x.id == this.deptform.controls['coordinatingIncharge'].value!
   );
   this.deptform.controls['coordinatingInchargeName'].setValue(
     inchargeId[0].firstName + ' ' + inchargeId[0].lastName
   );
 }

 getActiveUsers() {
   this.KemarService.get<any>(null, ServiceUrl.GetActiveUsers).subscribe(
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
}
