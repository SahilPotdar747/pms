import { ExcelServiceService } from 'src/app/service/excel-service.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { faSearch, faDownload, faTimes } from '@fortawesome/free-solid-svg-icons';
import { Table } from 'exceljs';
import { DataService } from 'src/app/service/data.service';
import { KemarServiceService } from 'src/app/service/kemar-service.service';
import { ServiceUrl } from 'src/app/service/service-url.service';
import { StatusDataService } from 'src/app/service/status-data.service';

@Component({
  selector: 'app-completed-task',
  templateUrl: './completed-task.component.html',
  styleUrls: ['./completed-task.component.scss']
})
export class CompletedTaskComponent {
  faTimes = faTimes;
  loading = false;
  @ViewChild('dt1') dt1: Table | undefined;
  @ViewChild('dt2') dt2: Table | undefined;
  totalCountOfMyDesk: any;
  totalCountOfMyTeam: any;
  pageSize = 15;
  currentPage = 1;
  IsMyTask = false;
  IsMyTeam = false;
  AllMyTaskData: any;
  AllMyTeamTaskData: any;
  filterMyTask = '';
  filterMyTeamTask = '';
  skiprow: number = 0;
  filter = '';
  selectedColumn: any;
  selectedColumn1: any;
  faSearch = faSearch;
  faDownload = faDownload;
  parentuserid: number = 0;
  selectedTask: number = 0;
  selectedProject: number = 0;
  selectedUserBy: number = 0;
  selectedUserTo: number = 0;
  selectedStatus: string = '';
  IsShowDDL1: boolean = false;
  isShowFilter1: boolean = true;
  IsShowDDL2: boolean = false;
  isShowFilter2: boolean = true;
  AllTaskTypeMaster: any;
  AllProjectMaster: any;
  AllAssignToUsers: any;
  GetAllAssignedBy: any;
  GetAllAssignedTo: any;
  AllStatus: any;
  myDepartment = 0;
  delegatedTaskStatusFilter = 'Completed';
  teamTaskStatusFilter = 'Completed';
  delegatedTaskFromDateFilter: any;
  delegatedTaskToDateFilter: any;
  teamTaskFromDateFilter: any;
  teamTaskToDateFilter: any;
  ReopenTaskForm!: FormGroup;
  reopenRemarkShow = false;
  submited = false;
  totalCountOfMyRaised: any;
  MyraisedTask: any;
  filterMyRaisedTask = '';
  selectedMyRaisedTask: number = 0;
    IsShowDDL3: boolean = false;
    selectedUserTo2: number = 0;
    selectedMyRaisedProject: number = 0;
    selectedMyRaisedStatus: string = '';
    isShowFilter3: boolean = true;
    AllMyRaisedStatus: any;

  constructor(
    private KemarService: KemarServiceService,
    private fb: FormBuilder,
    private statusData: StatusDataService,
    private dataService: DataService,
    private excel: ExcelServiceService
  ) {
    this.KemarService.isLoggedIn$ = true;
  }

  ngOnInit(): void {
    this.Reset();
    this.loading = true;
    this.loading = false;
    this.getTaskTransactionWithPagination(1);
    this.getTeamTaskTransactionWithPagination(1);
    this.checkIHaveTeam();
    this.getAllAssignedBy();
    this.getAllAssignedTo();
    this.getAllProjectMaster();
    this.getAllTaskTypeMaster();
    this.AllStatus = this.statusData.TaskStatusForFilter();
    this.AllMyRaisedStatus = this.statusData.TaskStatusForFilter();
    var date = new Date();
    this.delegatedTaskFromDateFilter = new Date(
      date.getFullYear(),
      date.getMonth(),
      1
    );
    this.delegatedTaskToDateFilter = date;
    var date = new Date();
    this.teamTaskFromDateFilter = new Date(
      date.getFullYear(),
      date.getMonth(),
      1
    );
    this.teamTaskToDateFilter = date;
    this.getAllRaisedTaskWithPagination(1);
  }

  get f1(): { [key: string]: AbstractControl } {
    return this.ReopenTaskForm.controls;
  }

  Reset() {
    this.ReopenTaskForm = this.fb.group({
      taskId: [0],
      remarks: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(100),
          this.noWhitespaceValidator,
        ],
      ],
      reopedBy: [''],
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

  pagechange(event: any) {
    
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getTaskTransactionWithPagination(1);
  }

  pagechange1(event: any) {
    
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getTeamTaskTransactionWithPagination(1);
  }

  pagechange2(event: any) {
    
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getAllRaisedTaskWithPagination(1);
  }

  OnResetDDL_Tab1() {
    //this.Reset();
    this.selectedUserBy = 0;
    this.filterMyTask = '';
    this.selectedProject = 0;
    this.selectedTask = 0;
    this.selectedStatus = '';
    this.getTaskTransactionWithPagination(1);
  }
  OnResetDDL_Tab2() {
    //this.Reset();
    this.selectedUserBy = 0;
    this.selectedUserTo = 0;
    this.filterMyTeamTask = '';
    this.selectedProject = 0;
    this.selectedTask = 0;
    this.selectedStatus = '';
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

  CloseFilter_Tab1() {
    this.IsShowDDL1 = false;
    this.isShowFilter1 = true;
  }

  CloseFilter_Tab2() {
    this.IsShowDDL2 = false;
    this.isShowFilter2 = true;
  }

  CloseFilter_Tab3() {
    this.isShowFilter3 = true;
    this.IsShowDDL3 = false;
  }

  onFilterTab1() {
    
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
    
    if ((this.isShowFilter3 = true)) {
      this.IsShowDDL3 = true;
      this.isShowFilter3 = false;
    } else {
      this.isShowFilter3 = false;
      this.IsShowDDL3 = false;
    }
  }

  Search() {
    
    this.currentPage = 1;
    this.getTaskTransactionWithPagination(1);
  }


  SearchForMyTeamTask() {
    this.currentPage = 1;
    this.getTeamTaskTransactionWithPagination(1);
  }

  SearchMyRaisedTask() {
    
    this.currentPage = 1;
    this.getAllRaisedTaskWithPagination(1);
  }
  getAllTaskTypeMaster() {
    
    this.loading = true;
    var query = {
      departmentId: this.myDepartment,
    };
    this.KemarService.get<any>(null, ServiceUrl.getAllTask, query).subscribe(
      (response) => {
        this.AllTaskTypeMaster = response;
        var all = {
          taskId: 0,
          taskName: 'Action Type',
        };
        this.AllTaskTypeMaster.unshift(all);
        this.loading = false;
      },
      (r) => {
        this.loading = false;
      }
    );
  }

  getAllRaisedByMeTask() {
    
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

  getAllAssignedBy() {
    
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

  getTaskTransactionWithPagination(currentPage: any) {
    
    var date = new Date();
    if (
      this.delegatedTaskFromDateFilter == null ||
      this.delegatedTaskToDateFilter == null
    ) {
      this.delegatedTaskFromDateFilter = new Date(
        date.getFullYear(),
        date.getMonth(),
        1
      );
      this.delegatedTaskToDateFilter = date;
    } else if (
      this.delegatedTaskFromDateFilter != null ||
      this.delegatedTaskToDateFilter != null
    ) {
      this.delegatedTaskFromDateFilter;
      this.delegatedTaskToDateFilter;
    } else this.delegatedTaskToDateFilter = date;
    this.loading = true;
    let query = {
      currentPage: currentPage,
      skiprow: this.skiprow,
      pagesize: this.pageSize,
      searchtext: this.filterMyTask,
      projectId: this.selectedProject,
      assignedById: this.selectedUserBy,
      taskTypeId: this.selectedTask,
      status: this.selectedStatus,
      fromDate: this.delegatedTaskFromDateFilter,
      toDate: this.delegatedTaskToDateFilter,
    };
    this.loading = true;
    this.KemarService.get<any>(
      null,
      ServiceUrl.getCompletedTaskWithPagination,
      query
    ).subscribe(
      (response) => {
        this.AllMyTaskData = response;
        if (response?.length > 0) {
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
    this.KemarService.get<any>(null, ServiceUrl.checkIHaveTeam).subscribe(
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

  getTeamTaskTransactionWithPagination(currentPage1: number) {
    
    var date = new Date();
    if (
      this.teamTaskFromDateFilter == null ||
      this.teamTaskToDateFilter == null
    ) {
      this.teamTaskFromDateFilter = new Date(
        date.getFullYear(),
        date.getMonth(),
        1
      );
      this.teamTaskToDateFilter = date;
    } else if (
      this.teamTaskFromDateFilter != null ||
      this.teamTaskToDateFilter != null
    ) {
      this.teamTaskFromDateFilter;
      this.teamTaskToDateFilter;
    } else this.teamTaskToDateFilter = date;
    this.loading = true;
    let query = {
      currentPage1: currentPage1,
      ParentUserId: this.parentuserid,
      skiprow1: this.skiprow,
      pagesize1: this.pageSize,
      searchtext1: this.filterMyTeamTask,
      projectId: this.selectedProject,
      assignedById: this.selectedUserBy,
      assignedTo: this.selectedUserTo,
      taskTypeId: this.selectedTask,
      status: this.selectedStatus,
      fromDate: this.teamTaskFromDateFilter,
      toDate: this.teamTaskToDateFilter,
    };
    this.loading = true;
    // if(this.IsMyTeam = false){
    //   return;
    // }
    this.KemarService.get<any>(
      null,
      ServiceUrl.getCompletedTeamTaskWithPagination,
      query
    ).subscribe(
      (response) => {
        this.AllMyTeamTaskData = response;
        if (response?.length > 0) {
          this.totalCountOfMyTeam = response[0].totalRecord;
        } else {
          this.totalCountOfMyTeam = 0;
        }
        this.currentPage = currentPage1;
      },
      (r) => {
        this.KemarService.ShowMessage('error', 'Error', r.error.errorMessage);
      }
    );
    this.loading = false;
  }

  getAllRaisedTaskWithPagination(currentPage: any) {
    
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
      ServiceUrl.getCompletedRaisedTaskWithPagination,
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

  DownloadTeamCompletedTask() {
    var date = new Date();
    if (
      this.teamTaskFromDateFilter == null ||
      this.teamTaskToDateFilter == null
    ) {
      this.teamTaskFromDateFilter = new Date(
        date.getFullYear(),
        date.getMonth(),
        1
      );
      this.teamTaskToDateFilter = date;
    } else if (
      this.teamTaskFromDateFilter != null ||
      this.teamTaskToDateFilter != null
    ) {
      this.teamTaskFromDateFilter;
      this.teamTaskToDateFilter;
    } else this.teamTaskToDateFilter = date;
    this.loading = true;
    let query = {
      searchtext1: this.filterMyTeamTask,
      projectId: this.selectedProject,
      assignedById: this.selectedUserBy,
      assignedTo: this.selectedUserTo,
      taskTypeId: this.selectedTask,
      status: this.selectedStatus,
      fromDate: this.teamTaskFromDateFilter,
      toDate: this.teamTaskToDateFilter,
    };
    this.loading = true;
    this.KemarService.get<any>(
      null,
      ServiceUrl.getCompletedTeamTaskToDownload,
      query
    ).subscribe(
      (response) => {
        let dataForExcel: unknown[][] = [];

        if (
          response == undefined ||
          response == null ||
          response?.length == 0
        ) {
          // Swal.fire('','Nothing to download','warning');
          return;
        }
        response.forEach((row: any) => {
          dataForExcel.push(Object.values(row));
        });

        let insideData = {
          title: 'My Team Completed Task',
          data: dataForExcel,
          headers: Object.keys(response[0]),
        };
        this.excel.exportExcelFoUserWiseReport(insideData);
      },
      (r) => {
        this.KemarService.ShowMessage('error', 'Error', r.error.errorMessage);
      }
    );
    this.loading = false;
  }

  DownloadMyCompletedTask() {
    var date = new Date();
    if (
      this.teamTaskFromDateFilter == null ||
      this.teamTaskToDateFilter == null
    ) {
      this.teamTaskFromDateFilter = new Date(
        date.getFullYear(),
        date.getMonth(),
        1
      );
      this.teamTaskToDateFilter = date;
    } else if (
      this.teamTaskFromDateFilter != null ||
      this.teamTaskToDateFilter != null
    ) {
      this.teamTaskFromDateFilter;
      this.teamTaskToDateFilter;
    } else this.teamTaskToDateFilter = date;
    this.loading = true;
    let query = {
      searchtext: this.filterMyTask,
      projectId: this.selectedProject,
      assignedById: this.selectedUserBy,
      taskTypeId: this.selectedTask,
      status: this.selectedStatus,
      fromDate: this.delegatedTaskFromDateFilter,
      toDate: this.delegatedTaskToDateFilter,
    };
    this.loading = true;
    this.KemarService.get<any>(
      null,
      ServiceUrl.getCompletedTaskToDownload,
      query
    ).subscribe(
      (response) => {
        let dataForExcel: unknown[][] = [];

        if (
          response == undefined ||
          response == null ||
          response?.length == 0
        ) {
          // Swal.fire('','Nothing to download','warning');
          return;
        }
        response.forEach((row: any) => {
          dataForExcel.push(Object.values(row));
        });

        let insideData = {
          title: 'My Completed Task',
          data: dataForExcel,
          headers: Object.keys(response[0]),
        };
        this.excel.exportExcelFoUserWiseReport(insideData);
      },
      (r) => {
        this.KemarService.ShowMessage('error', 'Error', r.error.errorMessage);
      }
    );
    this.loading = false;
  }

  openRemarkToReopenTask(task: any) {
    this.Reset();
    this.submited = false;
    this.ReopenTaskForm.controls['taskId'].setValue(task.taskId);
    this.reopenRemarkShow = true;
  }

  remarkPopUpSave() {
     this.submited = true;
     if (this.ReopenTaskForm.invalid) {
       return;
     }
     this.loading = true;
     var body = this.ReopenTaskForm.value;

     this.KemarService.postPatch<any>(
       ServiceUrl.reopenCompletedTask,
       body
     ).subscribe(
       (res) => {
         this.reopenRemarkShow = false;
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
         this.reopenRemarkShow = false;
         this.loading = false;
         this.KemarService.ShowMessage(
           'error',
           'Error',
           r.error.errorMessage == null ? r.error.message : r.error.errorMessage
         );
       }
     );
     this.loading = false;
  }
}
