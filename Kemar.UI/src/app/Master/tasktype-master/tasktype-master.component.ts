import { Component, OnInit, ViewChild } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { Table } from 'exceljs';
import { MessageService } from 'primeng/api';
import { ExcelServiceService } from 'src/app/service/excel-service.service';
import { KemarServiceService } from 'src/app/service/kemar-service.service';
import { ServiceUrl } from 'src/app/service/service-url.service';
import { StatusDataService } from 'src/app/service/status-data.service';

@Component({
  selector: 'app-tasktype-master',
  templateUrl: './tasktype-master.component.html',
  styleUrls: ['./tasktype-master.component.scss']
})
export class TasktypeMasterComponent {
 // KemarService: any;
 loading = false;
 @ViewChild('dt') dt: Table | undefined;
 title: string = "'Add ActionType'";
 taskform!: FormGroup;
 loginUser: string = '';
 submitted!: boolean;
 ShowActive: boolean = false;
 taskMaster: any;
 display: boolean = false;
 Save!: string;
 spinner: boolean = false;
 faSearch = faSearch;
 $index = 0;
 deptMaster: any;
 departmentMaster: any;
 actionDepartmentWise: any;
 taskStatus: any;
 filterActionList: string = '';

 //sorting
 public filter: string = '';
 pageSize = 15;
 skiprow: number = 0;
 currentPage = 1;
 totalRecords = 10;
 last: any;
 departmentFilter = 0;

 constructor(
   private KemarService: KemarServiceService,
   private excelService: ExcelServiceService,
   private statusData: StatusDataService,
   private fb: FormBuilder,
   private message: MessageService
 ) {
   this.KemarService.isLoggedIn$ = true;
 }

 ngOnInit(): void {
   //this.getAllTask();
   this.taskStatus = this.statusData.getProjectStatus();
   this.Reset();
   this.getAllDepartment();
   this.getAllTaskWithPagination(1);
   this.getAllActiveDept();
 }

 Reset() {
   const nonWhitespaceRegExp: RegExp = new RegExp("\\S");
   this.taskform = this.fb.group({
     taskId: [0, [Validators.required]],
     taskName: [
       '',
       [
         Validators.required,
         Validators.minLength(3),
         //Validators.pattern('[a-z A-Z-]*'),
         //Validators.pattern('[^-\\s][A-Za-z0-9()*%!_@./#&+:=-\\s]*'),
         Validators.maxLength(50),
         this.noWhitespaceValidator,
         //Validators.pattern(nonWhitespaceRegExp)
       ],
     ],
     nextTaskname: [''],
     nextTaskId: [''],
     departmentId: [''],
     isActive: [true],
   });
 }

 // public noWhitespaceValidator(control: FormControl) {
 //   const isWhitespace = (control.value || '').trim().length === 0;
 //   const isValid = !isWhitespace;
 //   return isValid ? null : { whitespace: true };
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
   this.taskform.reset();
   this.Reset();
 }

 ShowMessage(messageType: string, title: string, message: string) {
   this.message.add({
     severity: messageType,
     summary: title,
     detail: message,
   });
 }

 pagechange(event: any) {
   
   this.skiprow = event.first;
   this.pageSize = event.rows;
   this.getAllTaskWithPagination(1);
 }

 get f(): { [key: string]: AbstractControl } {
   return this.taskform.controls;
 }

 // applyFilterGlobal($event: any, stringVal: any) {
 //   this.dt!.filterGlobal(($event.target as HTMLInputElement).value, stringVal);
 // }

 getAllTask() {
   // this method not using for list
   this.loading = true;
   this.KemarService.get<any>(null, ServiceUrl.getAllTask).subscribe(
     (response) => {
       this.taskMaster = response;
     },
     (r) => {
       this.ShowMessage('error', 'Error', r.error.errorMessage);
     }
   );
   this.loading = false;
 }

 getAllDepartment() {
   // this method not using for list
   this.loading = true;
   this.KemarService.get<any>(null, ServiceUrl.getAllDepartment).subscribe(
     (response) => {
       this.departmentMaster = response;
       var all = {
         departmentId: 0,
         departmentName: 'All',
       };
       this.departmentMaster.unshift(all);
       this.loading = false;
     },
     (r) => {
       this.ShowMessage('error', 'Error', r.error.errorMessage);
     }
   );
   this.loading = false;
 }

 Search() {
   this.skiprow = 0;
   this.currentPage = 1;
   this.getAllTaskWithPagination(1);
 }

 getAllTaskWithPagination(currentPage: any) {
   
   var query = {
     currentPage: currentPage,
     skipRow: this.skiprow,
     rowSize: this.pageSize,
     departmentId: this.departmentFilter,
     searchtext: this.filterActionList,
   };
   this.loading = true;
   this.KemarService.get<any>(
     null,
     ServiceUrl.getAllTaskWithPagination,
     query
   ).subscribe(
     (response) => {
       this.taskMaster = response;
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
   this.taskform.controls['nextTaskname'].disable();
   this.Reset();
   this.ShowActive = true;
   this.title = 'Add ActionType';
   this.Save = 'Add';
   this.taskform.controls['nextTaskname'].setValue('');
   this.display = true;
 }

 edittask(task: any) {
   this.submitted = false;
   this.ShowActive = true;
   this.title = 'Update ActionType';
   this.taskform.patchValue(task);
   this.Save = 'Update';
   this.display = true;
   this.taskform.controls['nextTaskname'].enable();
   this.taskform.controls['nextTaskname'].setValue(task.taskTypeMaster.nextTaskname);
   this.ChangeDepartment();
 }

 registerTaskType() {
   
   this.submitted = true;
   if (this.taskform.invalid) {
     return;
   }
   this.loading = true;
   let body = this.taskform.value;
   this.KemarService.postPatch<any>(
     ServiceUrl.registerTaskType,
     body
   ).subscribe(
     (res) => {
       this.ShowMessage('success', 'Success', res.responseMessage);
       this.getAllTaskWithPagination(1);
     },
     (r) => {
       this.ShowMessage('error', 'Error', r.error.errorMessage);
     }
   );
   this.display = false;
   this.loading = false;
 }

 getAllActiveDept() {
   this.loading = true;
   this.KemarService.get<any>(null, ServiceUrl.getActiveDepartment).subscribe(
     (response) => {
       this.deptMaster = response;
       this.loading = false;
     },
     (r) => {
       this.KemarService.ShowMessage(
         'error',
         'Error',
         r.error.errorMessage == null ? r.error.message : r.error.errorMessage
       );
       this.loading = false;
     }
   );
 }

 ChangeDepartment() {
   var dept = this.taskform.get('departmentId')!.value;
    var id = this.taskform.get('taskId')!.value;
   this.taskform.controls['nextTaskname'].enable();
   this.taskform.controls['nextTaskname'].setValue('');
   if (dept != undefined || dept != null && this.taskMaster.length > 0){
     this.actionDepartmentWise = this.taskMaster.filter((x: {taskId: any; departmentId: any; }) => x.departmentId == dept && x.taskId != id);
   }
 }

 ChangeNextAction() {
   
   var dept = this.taskform.get('nextTaskname')!.value;
   if (dept != undefined || dept != null) {
     let actionName = this.actionDepartmentWise.filter(
       (x: { taskName: any }) => x.taskName == dept
     );
     this.taskform.controls['nextTaskId'].setValue(actionName[0].taskId);
   }
 }
}
