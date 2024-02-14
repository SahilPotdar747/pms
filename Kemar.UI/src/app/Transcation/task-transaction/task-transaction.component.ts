import { Component, OnInit, ViewChild } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { applyStyles, variationPlacements } from '@popperjs/core';
import { Table } from 'exceljs';
import { ExcelServiceService } from 'src/app/service/excel-service.service';
import { KemarServiceService } from 'src/app/service/kemar-service.service';
import { ServiceUrl } from 'src/app/service/service-url.service';
import { StatusDataService } from '../../service/status-data.service';
import { DataService } from 'src/app/service/data.service';
import {
  faSearch,
  faTemperatureHigh,
  faTimes,
  faUserInjured,
} from '@fortawesome/free-solid-svg-icons';
import { style } from '@angular/animations';
import { stringify } from 'querystring';


export class delegateModel {
  delegateHistoryId: number = 0;
  status: string = '';
  rejectRemarks: string = '';
}

export class reOpenRemarkModel {
  //taskId: number = 0;
  status: string = '';
  remarks: string = '';
}

@Component({
  selector: 'app-task-transaction',
  templateUrl: './task-transaction.component.html',
  styleUrls: ['./task-transaction.component.scss']
})
export class TaskTransactionComponent {
  parentuserid: number = 0;
  IsMyTeam = false;
  IsDelegatedTask = false;
  IsMyTask = false;
  TaskForm!: FormGroup;
  DelegateForm!: FormGroup;
  RemarkForm!: FormGroup;
  ReOpenForm!: FormGroup;
  myraisedDelegatedtask: any;
  AllMyTaskData: any;
  AllMyTeamTaskData: any;
  MyraisedTask: any;
  MyDelegatedTask: any;
  @ViewChild('dt1') dt1: Table | undefined;
  @ViewChild('dt2') dt2: Table | undefined;
  loading = false;
  submitted!: boolean;
  ShowActive: boolean = false;
  MyTask: any;
  display: boolean = false;
  MyTeamTask: any;
  assignto: any;
  title = 'Add New Task';
  SaveButtonTitle = 'Add';
  AllTaskTypeMaster: any;
  taskTypeMaster: any;
  AllProjectMaster: any;
  projectMaster: any;
  AllAssignToUsers: any;
  AllAssignToUsersForTeam: any;
  GetAllAssignedBy: any;
  GetAllAssignedTo: any;
  AllMyStatus: any;
  AllMyTeamStatus: any;
  AllMyRaisedStatus: any;
  AllStatus1: any;
  AllPriority: any;
  editingStatus = false;
  MyUserName = '';
  isSelfAssign = false;
  isTeamAssign = false;
  isRaisedByAssign = false;
  myTaskRowNo = 0;
  myTeamTaskRowNo = 0;
  IsEnableSearchBox: boolean = true;
  filter = '';
  filter1 = '';
  selectedColumn: any;
  selectedColumn1: any;
  faSearch = faSearch;
  faTimes = faTimes;
  selectedMyTask: number = 0;
  selectedMyTeamTask: number = 0;
  selectedMyRaisedTask: number = 0;
  selectedMyProject: number = 0;
  selectedMyTeamProject: number = 0;
  selectedMyRaisedProject: number = 0;
  selectedUserBy1: number = 0;
  selectedUserBy2: number = 0;
  selectedUserTo1: number = 0;
  selectedUserTo2: number = 0;
  selectedMyStatus: string = '';
  selectedMyTeamStatus: string = '';
  selectedMyRaisedStatus: string = '';
  rejectRemarkShow = false;
  rejectRemarks = '';
  reOpenRemarks = '';
  public delegateModelData: delegateModel = new delegateModel();
  public reOpenModelData: reOpenRemarkModel = new reOpenRemarkModel();
  delegatedTaskStatus: any;
  IsShowDDL1: boolean = false;
  isShowFilter1: boolean = true;
  IsShowDDL2: boolean = false;
  isShowFilter2: boolean = true;
  IsShowDDL3: boolean = false;
  isShowFilter3: boolean = true;
  taskHistoryDetail: any;
  showTaskHistory = false;
  taskDetails: any;
  showAction: boolean = false;
  showIcon: boolean = false;
  isShowEditIcon: boolean = true;

  //sorting MyTaskPage
  pageSize = 15;
  currentPage = 1;
  filterMyTask = '';
  filterMyTeamTask = '';
  filterMyRaisedTask = '';
  statusRowValue = '';

  last: any;
  skiprow: number = 0;

  // Dialoge
  displayDelegate = false;
  submited2 = false;

  // sorting MyteamTaskPage
  totalCountOfMyDesk: any;
  totalCountOfMyTeam: any;
  totalCountOfMyRaised: any;
  totalCountOfDelegateForMe: any;
  totalCountOfDelegateByMe: any;
  pageSize1 = 15;
  last1: any;
  skiprow1: number = 0;
  currentPage1 = 1;
  filterMyTask1 = '';
  filterMyTeamTask1 = '';
  statusRowValue1 = '';
  project: string = '';
  myDepartment = 0;

  // Filters
  // DelegatedTask Filter
  delegatedTaskStatusFilter = 'Requested';
  delegatedTaskFromDateFilter: any;
  delegatedTaskToDateFilter: any;

  // My Raised DelegatedTask Filter
  myRaisedDelegatedTaskStatusFilter = 'Requested';
  myRaisedDelegatedTaskFromDateFilter: any;
  myRaisedDelegatedTaskToDateFilter: any;
  currentdate!: string;
  displayReOpen: boolean = false;
  isStartDt: boolean = false;
  isEndDt: boolean = false;
  isUpdateMyTask: boolean = false;
  isUpdateRaisedTask: boolean = false;
  task: any;
  dataFilter: string[];

  constructor(
    private KemarService: KemarServiceService,
    private fb: FormBuilder,
    private statusData: StatusDataService,
    private dataService: DataService
  ) {
    this.KemarService.isLoggedIn$ = true;
    this.dataFilter = [''];
  }

  ngOnInit(): void {
    this.delegatedTaskStatus = this.statusData.getDelegatedTaskStatus();
    this.delegatedTaskStatusFilter = 'Requested';
    var date = new Date();
    console.log(date);
    this.delegatedTaskFromDateFilter = new Date(
      date.getFullYear(),
      date.getMonth(),
      1
    );
    this.delegatedTaskToDateFilter = date;
    this.myRaisedDelegatedTaskStatusFilter = 'Requested';
    this.myRaisedDelegatedTaskFromDateFilter = new Date(
      date.getFullYear(),
      date.getMonth(),
      1
    );
    this.currentdate = new Date().toISOString().split('T')[0];
    this.myRaisedDelegatedTaskToDateFilter = date;
    this.myDepartment = this.dataService.getUserDepartment();
    this.checkIHaveTeam();
    this.checkIHaveDelegatedTask();
    // this.getAllMytask();
    this.getTaskTransactionWithPagination(1);
    // this.getAllmyTeamTask();
    this.getTeamTaskTransactionWithPagination(1);
    this.getMyDelegatedTask(1);
    // this.getAllRaisedByMeTask();
    this.getAllRaisedTaskWithPagination(1);
    this.getAllRaisedDelegateTaskByMe(1);
    this.loading = true;
    this.getAllProjectMaster();
    this.getProjectMaster();
    this.getAllTaskTypeMaster();
    this.getTaskTypeMaster();
    this.getAllAssignToUsers();
    this.getAllAssignedBy();
    this.getAllAssignedTo();
    this.AllMyStatus = this.statusData.TaskStatusForFilter();
    this.AllMyTeamStatus = this.statusData.TaskStatusForFilter();
    this.AllMyRaisedStatus = this.statusData.TaskStatusForFilter();
    this.AllStatus1 = this.statusData.getTaskStatus();
    this.AllPriority = this.statusData.getTaskPriority();
    this.Reset();
    this.ResetDelegate();
    this.loading = false;
    this.MyUserName = this.dataService.getUserName();
    // console.log(this.MyUserName)
  }

  get f(): { [key: string]: AbstractControl } {
    return this.TaskForm.controls;
  }

  pagechange(event: any) {
    // 
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getTaskTransactionWithPagination(1);
  }

  pagechange1(event: any) {
    // 
    this.skiprow1 = event.first;
    this.pageSize1 = event.rows;
    this.getTeamTaskTransactionWithPagination(1);
  }

  pagechange2(event: any) {
    // 
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getAllRaisedTaskWithPagination(1);
  }

  pagechange3(event: any) {
    // 
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getMyDelegatedTask(1);
  }

  pagechange4(event: any) {
    // 
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getAllRaisedDelegateTaskByMe(1);
  }

  Reset() {
    this.TaskForm = this.fb.group({
      taskId: [0, [Validators.required]],
      title: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(100),
          Validators.pattern('[^-\\s][A-Za-z0-9()*%!_@./#&+:=-\\s]*'),
          this.noWhitespaceValidator,
        ],
      ],
      projectId: ['', [Validators.required]],
      taskTypeId: ['', [Validators.required]],
      description: ['', [Validators.maxLength(1000)]],
      priority: ['', [Validators.required]],
      assignedToId: [0, [Validators.required]],
      assignedTo: ['', [Validators.required]],
      assignedById: [0, [Validators.required]],
      assignedBy: [''],
      assignedDate: [''],
      exceptedStartDate: ['', [Validators.required]],
      exceptedEndDate: ['', [Validators.required]],
      // validator: this.dateLessThan('exceptedStartDate', 'exceptedEndDate'),
      actualStartDate: [''],
      actualEndDate: [''],
      status: ['', [Validators.required]],
      remarks: [''],
      isActive: [true],
    });
  }

  onReset() {
    this.submitted = false;
    this.TaskForm.reset();
    this.Reset();
  }

  // public noWhitespaceValidator(control: FormControl) {
  //   const isWhitespace = (control.value || '').trim().length === 0;
  //   const isValid = !isWhitespace;
  //   return isValid ? null : { whitespace: true };
  // }

  KeyPressDate = (event: any) => this.KemarService.keyonDate(event);

  ResetDelegate() {
    this.DelegateForm = this.fb.group({
      raisedBy: [''],
      taskId: [0, [Validators.required]],
      transferToId: ['', [Validators.required]],
      remarks: ['', [Validators.required,this.noWhitespaceValidator]],
      
    });
  }

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

  CloseFilter_Tab1() {
    this.isShowFilter1 = true;
    this.IsShowDDL1 = false;
  }
  CloseFilter_Tab2() {
    this.isShowFilter2 = true;
    this.IsShowDDL2 = false;
  }
  CloseFilter_Tab3() {
    this.isShowFilter3 = true;
    this.IsShowDDL3 = false;
  }

  onFilterTab1() {
    // 
    if ((this.isShowFilter1 = true)) {
      this.IsShowDDL1 = true;
      this.isShowFilter1 = false;
    } else {
      this.isShowFilter1 = false;
      this.IsShowDDL1 = false;
    }
  }

  onFilterTab2() {
    
    if ((this.isShowFilter2 = true)) {
      this.IsShowDDL2 = true;
      this.isShowFilter2 = false;
    } else {
      this.isShowFilter2 = false;
      this.IsShowDDL2 = false;
    }
  }

  onFilterTab3() {
    // 
    if ((this.isShowFilter3 = true)) {
      this.IsShowDDL3 = true;
      this.isShowFilter3 = false;
    } else {
      this.isShowFilter3 = false;
      this.IsShowDDL3 = false;
    }
  }

  get f1(): { [key: string]: AbstractControl } {
    return this.DelegateForm.controls;
  }

  get f2(): { [key: string]: AbstractControl } {
    return this.ReOpenForm.controls;
  }

  SearchFilter1() {
    // 
    this.currentPage1 = 1;
    this.AllProjectMaster;
    this.getTeamTaskTransactionWithPagination(this.currentPage);
  }

  // Have to Implement
  getAllTaskTypeMaster() {
    this.loading = true;
    var query = {
      departmentId: this.myDepartment,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.getAllTaskTypeDepartmentWise,
      query
    ).subscribe(
      (response) => {
        this.AllTaskTypeMaster = response;
        this.taskTypeMaster = response;
        var all = {
          taskId: 0,
          taskName: 'Task Type',
        };
        this.AllTaskTypeMaster.unshift(all);
        this.loading = false;
      },
      (r) => {
        this.loading = false;
      }
    );
  }

  getTaskTypeMaster() {
    this.loading = true;
    var query = {
      departmentId: this.myDepartment,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.getAllTaskTypeDepartmentWise,
      query
    ).subscribe(
      (response) => {
        this.taskTypeMaster = response;
        this.loading = false;
      },
      (r) => {
        this.loading = false;
      }
    );
  }

  getAllAssignedBy() {
    // 
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.GetActiveUsers).subscribe(
      (response) => {
        this.GetAllAssignedBy = response;
        var all = {
          id: 0,
          userName: 'Assign by User',
        };
        this.GetAllAssignedBy.unshift(all);
        this.loading = false;
      },
      (r) => {
        this.loading = false;
      }
    );
  }

  getAllAssignedTo() {
    // 
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.getUsersData).subscribe(
      (response) => {
        this.GetAllAssignedTo = response;
        var all = {
          id: 0,
          userName: 'Assign to User',
        };
        this.GetAllAssignedTo.unshift(all);
        this.loading = false;
      },
      (r) => {
        this.loading = false;
      }
    );
  }

  getAllProjectMaster() {
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.getActiveProject).subscribe(
      (response) => {
        this.AllProjectMaster = response;
        var all = {
          projectId: 0,
          projectName: 'Project',
        };
        this.AllProjectMaster.unshift(all);
        this.loading = false;
      },
      (r) => {
        this.loading = false;
      }
    );
  }

  getProjectMaster() {
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.getActiveProject).subscribe(
      (response) => {
        this.projectMaster = response;
        this.loading = false;
      },
      (r) => {
        this.loading = false;
      }
    );
  }

  getAllAssignToUsers() {
    this.loading = true;
    this.KemarService.get<any>(
      null,
      ServiceUrl.GetAllUsersOfParentUserAsync
    ).subscribe(
      (response) => {
        this.AllAssignToUsers = response;

        var myId = this.dataService.getUserId();
        this.AllAssignToUsersForTeam =response.filter((x: { id: any; })  => x.id != myId);
        this.loading = false;
      },
      (r) => {
        this.loading = false;
      }
    );
  }

  getAllMytask() {
    this.loading = true;

    this.KemarService.get<any>(null, ServiceUrl.GetAllMyTask).subscribe(
      (response) => {
        this.AllMyTaskData = response;
        this.loading = false;
      },
      (r) => {
        this.loading = false;
        if (r.status == '401' && r.statusText == 'Unauthorized') {
          this.KemarService.ShowMessage(
            'error',
            r.statusText,
            'Please Login again'
          );
        }
      }
    );
  }

  getTaskTransactionWithPagination(currentPage: any) {
    // 
    let query = {
      currentPage: currentPage,
      skiprow: this.skiprow,
      pagesize: this.pageSize,
      searchtext: this.filterMyTask,
      projectId: this.selectedMyProject,
      assignedById: this.selectedUserBy1,
      taskTypeId: this.selectedMyTask,
      status: this.selectedMyStatus,
    };
    this.loading = true;
    this.KemarService.get<any>(
      null,
      ServiceUrl.getTaskTransactionWithPagination,
      query
    ).subscribe(
      (response) => {
        this.AllMyTaskData = response;
        if (response.length > 0) {
          this.totalCountOfMyDesk = response[0].totalRecord;
        } else {
          this.totalCountOfMyDesk = 0;
        }
        this.currentPage = currentPage;
      },
      (r) => {
        this.KemarService.ShowMessage('error', 'Error', r.error.errorMessage);
      }
    );
    this.loading = false;
  }

  checkIHaveTeam() {
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.CheckIHaveTeam).subscribe(
      (response) => {
        this.IsMyTeam = response;
        this.loading = false;
      },
      (r) => {
        this.loading = false;
        this.IsMyTeam = false;
        if (r.status == '401' && r.statusText == 'Unauthorized') {
          this.KemarService.ShowMessage(
            'error',
            r.statusText,
            'Please Login again'
          );
        }
      }
    );
  }

  getAllmyTeamTask() {
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.GetAllMyTeamTask).subscribe(
      (response) => {
        this.AllMyTeamTaskData = response;
        console.log(response);
        this.loading = false;
      },
      (r) => {
        this.loading = false;
        if (r.status == '401' && r.statusText == 'Unauthorized') {
          this.KemarService.ShowMessage(
            'error',
            r.statusText,
            'Please Login again'
          );
        }
      }
    );
  }

  getTeamTaskTransactionWithPagination(currentPage1: number) {
    // 
    let query = {
      currentPage1: currentPage1,
      ParentUserId: this.parentuserid,
      skiprow1: this.skiprow1,
      pagesize1: this.pageSize1,
      searchtext1: this.filterMyTeamTask,
      projectId: this.selectedMyTeamProject,
      assignedById: this.selectedUserBy2,
      assignedTo: this.selectedUserTo1,
      taskTypeId: this.selectedMyTeamTask,
      status: this.selectedMyTeamStatus,
    };
    this.loading = true;
    // if(this.IsMyTeam = false){
    //   return;
    // }
    this.KemarService.get<any>(
      null,
      ServiceUrl.getTeamTaskTransactionWithPagination,
      query
    ).subscribe(
      (response) => {
        this.AllMyTeamTaskData = response;
        if (response.length > 0) {
          this.totalCountOfMyTeam = response[0].totalRecord;
        } else {
          this.totalCountOfMyTeam = 0;
        }
        this.currentPage1 = currentPage1;
      },
      (r) => {
        this.KemarService.ShowMessage('error', 'Error', r.error.errorMessage);
      }
    );
    this.loading = false;
  }

  OnResetDDL_Tab1() {
    //this.Reset();
    this.selectedUserBy1 = 0;
    this.filterMyTask = '';
    this.selectedMyProject = 0;
    this.selectedMyTask = 0;
    this.selectedMyStatus = '';
    this.getTaskTransactionWithPagination(1);
  }
  OnResetDDL_Tab2() {
    //this.Reset();
    this.selectedUserBy2 = 0;
    this.selectedUserTo1 = 0;
    this.filterMyTeamTask = '';
    this.selectedMyTeamProject = 0;
    this.selectedMyTeamTask = 0;
    this.selectedMyTeamStatus = '';
    this.getTeamTaskTransactionWithPagination(1);
  }
  OnResetDDL_Tab3() {
    //this.Reset();
    this.selectedUserTo2 = 0;
    this.filterMyRaisedTask = '';
    this.selectedMyRaisedProject = 0;
    this.selectedMyRaisedTask = 0;
    this.selectedMyRaisedStatus = '';
    this.getAllRaisedTaskWithPagination(1);
  }
  OpenSelfTask() {
    // 
    this.submitted = false;
    this.title = 'Add New Task';
    this.SaveButtonTitle = 'Add';
    this.isStartDt = true;
    this.isEndDt = true;
    this.isUpdateMyTask = true;
    this.isUpdateRaisedTask = false;
    this.isRaisedByAssign = true;
    this.Reset();
    this.TaskForm.controls['assignedTo'].setValue(this.dataService.getUserId());
    this.TaskForm.controls['status'].setValue('New Task');
    // this.TaskForm.controls['title'].enable();
    this.display = true;
    this.isSelfAssign = true;
  }

  OpenTeamTask() {
    this.submitted = false;
    this.title = 'Add New Task';
    this.SaveButtonTitle = 'Add';
    this.isStartDt = true;
    this.isEndDt = true;
    this.isUpdateMyTask = true;
    this.isUpdateRaisedTask = false;
    this.isSelfAssign = false;
    this.isRaisedByAssign = false;
    this.Reset();
    this.TaskForm.controls['status'].setValue('New Task');
    // this.TaskForm.controls['title'].enable();
    this.display = true;
  }

  editMyTask(task: any) {
    // this.isSelfAssign = true;
    this.isUpdateMyTask = true;
    this.isUpdateRaisedTask = false;
    this.SaveButtonTitle = 'Update';
    this.title = 'Update Task';
    this.isSelfAssign = true;
    this.isTeamAssign = true;
    this.isRaisedByAssign = true
    this.isStartDt = true;
    this.isEndDt = true;
    this.Reset();
    this.TaskForm.patchValue(task);
    this.TaskForm.controls['exceptedStartDate'].setValue(
      task.exceptedStartDate?.replace('T', ' ')
    );
    this.TaskForm.controls['exceptedEndDate'].setValue(
      task.exceptedEndDate?.replace('T', ' ')
    );
    // this.TaskForm.controls['title'].disable();
    this.display = true;
    // this.editing = true;
  }

  editMyTeamTask(task: any) {
    // this.isSelfAssign = true;
    this.isUpdateMyTask = true;
    this.isUpdateRaisedTask = false;
    this.SaveButtonTitle = 'Update';
    this.title = 'Update Task';
    this.isSelfAssign = true;
    this.isTeamAssign = false;
    this.isRaisedByAssign = true;
    this.isStartDt = true;
    this.isEndDt = true;
    this.Reset();
    this.TaskForm.patchValue(task);
    this.TaskForm.controls['exceptedStartDate'].setValue(
      task.exceptedStartDate?.replace('T', ' ')
    );
    this.TaskForm.controls['exceptedEndDate'].setValue(
      task.exceptedEndDate?.replace('T', ' ')
    );
    // this.TaskForm.controls['title'].disable();
    this.display = true;
    // this.editing = true;
  }

  editMyRaisedTask(task: any) {
    // 
    // this.isSelfAssign = true;
    this.isUpdateMyTask = false;
    this.isUpdateRaisedTask = true;
    this.SaveButtonTitle = 'Update';
    this.title = 'Update Task';
    this.isSelfAssign = true;
    this.isTeamAssign = true;
    this.isRaisedByAssign = false;
    this.Reset();
    this.TaskForm.controls['exceptedStartDate'].clearValidators();
    this.TaskForm.controls['exceptedEndDate'].clearValidators();
    this.TaskForm.controls['exceptedStartDate'].updateValueAndValidity();
    this.TaskForm.controls['exceptedEndDate'].updateValueAndValidity();
    this.TaskForm.controls['assignedTo'].setValue(task.assignedTo);
    if(task.assignedTo > 0){
      this.isStartDt = true;
    this.isEndDt = true;
    }
    else{
      this.isStartDt = false;
      this.isEndDt = false;
    }
    this.TaskForm.patchValue(task);
    this.TaskForm.controls['exceptedStartDate'].setValue(
      task.exceptedStartDate?.replace('T', ' ')
    );
    this.TaskForm.controls['exceptedEndDate'].setValue(
      task.exceptedEndDate?.replace('T', ' ')
    );
    // this.TaskForm.controls['title'].disable();
    this.display = true;
    // this.editing = true;
  }

  ChangeAssignedTo() {
    if (
      this.TaskForm.get('assignedTo')!.value != '' &&
      this.TaskForm.get('assignedTo')!.value != 0
    ) {
      this.TaskForm.controls['exceptedStartDate'].setValidators([
        Validators.required,
      ]);
      this.TaskForm.controls['exceptedEndDate'].setValidators([
        Validators.required,
      ]);
      this.TaskForm.controls['exceptedStartDate'].updateValueAndValidity();
      this.TaskForm.controls['exceptedEndDate'].updateValueAndValidity();
      this.isStartDt = true;
      this.isEndDt = true;
    } else {
      this.TaskForm.controls['exceptedStartDate'].clearValidators();
      this.TaskForm.controls['exceptedEndDate'].clearValidators();
      this.TaskForm.controls['exceptedStartDate'].updateValueAndValidity();
      this.TaskForm.controls['exceptedEndDate'].updateValueAndValidity();
      this.isStartDt = false;
      this.isEndDt = false;
    }
  }
  Search() {
    // 
    this.currentPage = 1;
    this.getTaskTransactionWithPagination(1);
  }

  SearchForMyTeamTask() {
    this.currentPage1 = 1;
    this.getTeamTaskTransactionWithPagination(1);
  }

  SearchMyRaisedTask() {
    // 
    this.currentPage = 1;
    this.getAllRaisedTaskWithPagination(1);
  }

  // dateLessThan(exceptedStartDate: string, exceptedEndDate: string) {
  //   
  //   return (group: FormGroup): {[key: string]: any} => {
  //    let f = group.controls[exceptedStartDate];
  //    let t = group.controls[exceptedEndDate];
  //    if (f.value < t.value) {
  //      return {
  //        dates: "exceptedEndDate should be greater than exceptedStartDate"
  //      };
  //    }
  //    return {};
  //   }
  // }

  dateCheck(task: any) {
    // 
    let startDate = this.TaskForm.controls['exceptedStartDate'].setValue(task.exceptedStartDate?.replace('T', ' '));
    let endDate = this.TaskForm.controls['exceptedEndDate'].setValue(task.exceptedEndDate?.replace('T', ' '));
    if (startDate > endDate) {
      alert("End date need to be bigger then start date");
    }
  }


  updateTask() {
    // 
    // this.isUpdateMyTask = true;
    // this.isUpdateRaisedTask = false;
    this.submitted = true;
    if (this.TaskForm.invalid) {
      return;
    }
    this.loading = true;
    var body = this.TaskForm.value;
    let startDate = this.TaskForm.controls['exceptedStartDate'];
    let endDate = this.TaskForm.controls['exceptedEndDate'];
    if (startDate.value > endDate.value) {
      this.KemarService.ShowMessage(
        'error',
        'Error',
        "End date should be greater than start date"
      );
      this.loading = false;
      return;
    }
    this.KemarService.postPatch<any>(ServiceUrl.RegisterTask, body).subscribe(
      (res) => {
        //   const date1 = this.TaskForm.get('exceptedStartDate')?.value;
        //   const date2 = this.TaskForm.get('exceptedEndDate')?.value;
        //   if ((date1 !== null && date2 !== null) && date1 > date2) {
        //     return;
        // }
        this.display = false;
        this.loading = false;
        this.KemarService.ShowMessage(
          'success',
          'Success',
          res.responseMessage
        );
        this.getTaskTransactionWithPagination(1);
        this.getTeamTaskTransactionWithPagination(1);
        this.getAllRaisedTaskWithPagination(1);
      },
      (r) => {
        this.display = false;
        this.loading = false;
        this.KemarService.ShowMessage(
          'error',
          'Error',
          r.error.errorMessage == null ? r.error.message : r.error.errorMessage
        );
        // if ((this.editingStatus = true)) {
        //   this.getAllMytask();
        //   this.getAllmyTeamTask();
        // }
      }
    );
  }

  updateRaisedTask() {
    // 
    this.submitted = true;
    if (this.TaskForm.invalid) {
      return;
    }
    this.loading = true;
    var body = this.TaskForm.value;
    let startDate = this.TaskForm.controls['exceptedStartDate'];
    let endDate = this.TaskForm.controls['exceptedEndDate'];
    if (startDate.value > endDate.value) {
      this.KemarService.ShowMessage(
        'error',
        'Error',
        "End date should be greater than start date"
      );
      this.loading = false;
      return;
    }
    this.KemarService.postPatch<any>(ServiceUrl.RegisterTask, body).subscribe(
      (res) => {
        //   const date1 = this.TaskForm.get('exceptedStartDate')?.value;
        //   const date2 = this.TaskForm.get('exceptedEndDate')?.value;
        //   if ((date1 !== null && date2 !== null) && date1 > date2) {
        //     return;
        // }
        this.display = false;
        this.loading = false;
        this.KemarService.ShowMessage(
          'success',
          'Success',
          res.responseMessage
        );
        this.getTaskTransactionWithPagination(1);
        this.getTeamTaskTransactionWithPagination(1);
        this.getAllRaisedTaskWithPagination(1);
      },
      (r) => {
        this.display = false;
        this.loading = false;
        this.KemarService.ShowMessage(
          'error',
          'Error',
          r.error.errorMessage == null ? r.error.message : r.error.errorMessage
        );
        // if ((this.editingStatus = true)) {
        //   this.getAllMytask();
        //   this.getAllmyTeamTask();
        // }
      }
    );
  }

  OnSelectItem() {
    // 
    // var status = this.TaskForm.get('status')?.value;
    // if(status == "pending")
    // this.isShowEditIcon = true;
    // else
    // this.isShowEditIcon = false;
  }

  openStatusOfMyTeam(rowNo: any, row: any) {
    // 
    this.editingStatus = true;
    if (this.myTeamTaskRowNo != rowNo) {
      this.myTeamTaskRowNo = rowNo;
      this.statusRowValue = row.status;
    }
  }

  saveStatusOfMyTeam(task: any) {
    // 
    this.TaskForm.patchValue(task);
    this.TaskForm.controls['status'].setValue(this.statusRowValue);
    this.updateTask();
    this.editingStatus = false;
    this.statusRowValue = '';
    this.myTeamTaskRowNo = 0;
    this.myTaskRowNo = 0;
  }

  closeMyStatus() {
    this.getTaskTransactionWithPagination(1);
  }

  closeMyTeamStatus() {
    this.getTeamTaskTransactionWithPagination(1);
  }

  OpenDelegate(task: any) {
    this.submited2 = false;
    this.ResetDelegate();
    this.DelegateForm.controls['taskId'].setValue(task.taskId);
    this.displayDelegate = true;
  }

  updateDelegatedTask() {
    this.submited2 = true;
    if (this.DelegateForm.invalid) {
      return;
    }
    this.loading = true;
    var body = this.DelegateForm.value;

    this.KemarService.postPatch<any>(
      ServiceUrl.RegisterDeleateTask,
      body
    ).subscribe(
      (res) => {
        this.displayDelegate = false;
        this.loading = false;
        this.KemarService.ShowMessage(
          'success',
          'Success',
          res.responseMessage
        );
        this.getTaskTransactionWithPagination(1);
        this.getTeamTaskTransactionWithPagination(1);
        this.getAllRaisedDelegateTaskByMe(1);
      },
      (r) => {
        this.displayDelegate = false;
        this.loading = false;
        this.KemarService.ShowMessage(
          'error',
          'Error',
          r.error.errorMessage == null ? r.error.message : r.error.errorMessage
        );
        // if (this.editingStatus = true) {
        //   this.getAllMytask();
        //   this.getAllmyTeamTask();
        // }
      }
    );
    this.submited2 = false;
  }


  checkIHaveDelegatedTask() {
    this.loading = true;
    this.KemarService.get<any>(
      null,
      ServiceUrl.checkIHaveDelegatedTask
    ).subscribe(
      (response) => {
        this.IsDelegatedTask = response;
        this.loading = false;
      },
      (r) => {
        this.loading = false;
        this.IsMyTeam = false;
        if (r.status == '401' && r.statusText == 'Unauthorized') {
          this.KemarService.ShowMessage(
            'error',
            r.statusText,
            'Please Login again'
          );
        }
      }
    );
  }
  // MyDelegatedTask

  getMyDelegatedTask(currentPage: any) {
    // 

    if (this.delegatedTaskStatusFilter != 'Requested') {
      if (
        this.delegatedTaskFromDateFilter == null ||
        this.delegatedTaskToDateFilter == null
      ) {
        this.KemarService.ShowMessage(
          'warn',
          'warn',
          'Select From Date and To Date to Search'
        );
        return;
      }
      this.showAction = false;
      this.showIcon = false;
    } else {
      this.showAction = true;
      this.showIcon = true;
    }
    this.loading = true;
    let query = {
      currentPage: currentPage,
      taskStatus: this.delegatedTaskStatusFilter,
      fromDate: this.delegatedTaskFromDateFilter,
      toDate: this.delegatedTaskToDateFilter,
      skipRow: this.skiprow,
      rowSize: this.pageSize,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.getAllDeleatedTask,
      query
    ).subscribe(
      (response) => {
        this.MyDelegatedTask = response;
        if (response.length > 0) {
          this.totalCountOfDelegateForMe = response[0].totalRecord;
        } else {
          this.totalCountOfDelegateForMe = 0;
        }
        this.currentPage = currentPage;
        this.loading = false;
      },
      (r) => {
        this.loading = false;
        if (r.status == '401' && r.statusText == 'Unauthorized') {
          this.KemarService.ShowMessage(
            'error',
            r.statusText,
            'Please Login again'
          );
        }
      }
    );
  }

  saveDelegate(delegateData: any, status: string) {
    // 
    this.submited2 = false;
    this.delegateModelData.delegateHistoryId = delegateData.delegateHistoryId;
    this.delegateModelData.status = status;
    this.delegateModelData.rejectRemarks = this.rejectRemarks;
    if (status == 'Rejected')
     {
      this.rejectRemarkShow = true;
        return;
    } 
    else 
    {
      this.rejectRemarkShow = false;
    }
    this.UpdateDelegatedTaskAction();
  }

  remarkPopUpSave() {
    // 
    this.submited2 = true;
      if(this.rejectRemarks.length === 0){
        return;
      }
    this.delegateModelData.rejectRemarks = this.rejectRemarks;
    this.UpdateDelegatedTaskAction();
  }

  delegateRemarksClose() {
    this.rejectRemarkShow = false;
    this.rejectRemarks = '';
  }

  UpdateDelegatedTaskAction() {
    // 
    this.loading = true;
    this.KemarService.postPatch<any>(
      ServiceUrl.UpdateDeleatedTaskAction,
      this.delegateModelData
    ).subscribe(
      (res) => {
        this.loading = false;
        this.KemarService.ShowMessage(
          'success',
          'Success',
          res.responseMessage
        );
        this.rejectRemarks = '';
        this.rejectRemarkShow = false;
        this.getMyDelegatedTask(1);
      },
      (r) => {
        this.loading = false;
        this.KemarService.ShowMessage(
          'error',
          'Error',
          r.error.errorMessage == null ? r.error.message : r.error.errorMessage
        );
        this.rejectRemarkShow = false;
        this.rejectRemarks = '';
      }
    );
  }

  getAllRaisedByMeTask() {
    // 
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.getAllRaisedByMeTask).subscribe(
      (response) => {
        this.MyraisedTask = response;
        if (response.length > 0) {
          this.totalCountOfMyRaised = response[0].totalRecord;
        } else {
          this.totalCountOfMyRaised = 0;
        }
        this.loading = false;
      },
      (r) => {
        this.loading = false;
        if (r.status == '401' && r.statusText == 'Unauthorized') {
          this.KemarService.ShowMessage(
            'error',
            r.statusText,
            'Please Login again'
          );
        }
      }
    );
  }

  getAllRaisedTaskWithPagination(currentPage: any) {
    // 
    let query = {
      currentPage: currentPage,
      skiprow: this.skiprow,
      pagesize: this.pageSize,
      searchtext: this.filterMyRaisedTask,
      projectId: this.selectedMyRaisedProject,
      assignedToId: this.selectedUserTo2,
      taskTypeId: this.selectedMyRaisedTask,
      status: this.selectedMyRaisedStatus,
    };
    this.loading = true;
    this.KemarService.get<any>(
      null,
      ServiceUrl.getAllRaisedTaskWithPagination,
      query
    ).subscribe(
      (response) => {
        this.MyraisedTask = response;
        if (response.length > 0) {
          this.totalCountOfMyRaised = response[0].totalRecord;
        } else {
          this.totalCountOfMyRaised = 0;
        }
        this.currentPage = currentPage;

      },
      (r) => {
        this.KemarService.ShowMessage('error', 'Error', r.error.errorMessage);
      }
    );
    this.loading = false;
  }

  getAllRaisedDelegateTaskByMe(currentPage: any) {
    this.loading = true;
    if (
      this.myRaisedDelegatedTaskFromDateFilter == null ||
      this.myRaisedDelegatedTaskToDateFilter == null
    ) {
      this.KemarService.ShowMessage(
        'warn',
        'warn',
        'Select From Date and To Date to Search'
      );
      return;
    }

    this.loading = true;
    let query = {
      currentPage: currentPage,
      taskStatus: this.myRaisedDelegatedTaskStatusFilter,
      fromDate: this.myRaisedDelegatedTaskFromDateFilter,
      toDate: this.myRaisedDelegatedTaskToDateFilter,
      skipRow: this.skiprow,
      rowSize: this.pageSize,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.getMyRaisedDelegatedTask,
      query
    ).subscribe(
      (response) => {
        this.myraisedDelegatedtask = response;
        if (response.length > 0) {
          this.totalCountOfDelegateByMe = response[0].totalRecord;
        } else {
          this.totalCountOfDelegateByMe = 0;
        }
        this.currentPage = currentPage;
        this.loading = false;
      },
      (r) => {
        this.loading = false;
        if (r.status == '401' && r.statusText == 'Unauthorized') {
          this.KemarService.ShowMessage(
            'error',
            r.statusText,
            'Please Login again'
          );
        }
      }
    );
  }

  getTaskHistory(task: any, taskID: number) {
    // 
    this.loading = true;
    this.taskDetails = task;
    let query = {
      taskId: taskID,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.getTaskHistory,
      query
    ).subscribe(
      (response) => {
        this.taskHistoryDetail = response.reverse();
        this.showTaskHistory = true;
        this.loading = false;
      },
      (r) => {
        this.loading = false;
        if (r.status == '401' && r.statusText == 'Unauthorized') {
          this.KemarService.ShowMessage(
            'error',
            r.statusText,
            'Please Login again'
          );
        }
      }
    );
  }

}
