import { Component, OnInit } from '@angular/core';
import { faSearch, faDownload, faTimes } from '@fortawesome/free-solid-svg-icons';
import { KemarServiceService } from 'src/app/service/kemar-service.service';
import { ServiceUrl } from 'src/app/service/service-url.service';
import { StatusDataService } from 'src/app/service/status-data.service';
import { ExcelServiceService } from 'src/app/service/excel-service.service';
@Component({
  selector: 'app-supporting-incharge',
  templateUrl: './supporting-incharge.component.html',
  styleUrls: ['./supporting-incharge.component.scss']
})
export class SupportingInchargeComponent {
  faTimes=faTimes;
  loading = false;
  AllMyTeamTaskData: any;
  totalCountOfMyTeam: any;
  parentuserid: number = 0;
  pageSize1 = 15;
  skiprow1: number = 0;
  currentPage1 = 1;
  filterMyTeamTask = '';
  selectedTask: number = 0;
  selectedProject: number = 0;
  selectedUserBy: number = 0;
  selectedUserTo: number = 0;
  selectedStatus: string = '';
  IsShowDDL2: boolean = false;
  isShowFilter2: boolean = true;
  AllTaskTypeMaster: any;
  taskTypeMaster: any;
  AllProjectMaster: any;
  AllAssignToUsers: any;
  GetAllAssignedBy: any;
  GetAllAssignedTo: any;
  AllStatus: any;
  supportingInchargeFromDateFilter: any;
  supportingInchargeToDateFilter: any;
  faSearch = faSearch;
  faDownload = faDownload;

  constructor(
    private KemarService: KemarServiceService,
    private statusData: StatusDataService,
    private excel: ExcelServiceService
  ) {
    this.KemarService.isLoggedIn$ = true;
  }

  ngOnInit(): void {
    this.getTeamTaskTransactionWithPagination(1);
    this.getAllAssignedBy();
    this.getAllAssignedTo();
    this.getAllProjectMaster();
    this.getAllTaskTypeMaster();
    this.AllStatus = this.statusData.TaskStatusForFilter();
    var date = new Date();
    this.supportingInchargeFromDateFilter = new Date(
      date.getFullYear(),
      date.getMonth(),
      1
    );
    this.supportingInchargeToDateFilter = date;
  }

  pagechange1(event: any) {
    
    this.skiprow1 = event.first;
    this.pageSize1 = event.rows;
    this.getTeamTaskTransactionWithPagination(1);
  }

  CloseFilter_Tab2(){
    this.isShowFilter2 = true;
    this.IsShowDDL2 = false;
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

  onFilterTab2() {
    
    if ((this.isShowFilter2 = true)) {
      this.IsShowDDL2 = true;
      this.isShowFilter2 = false;
    } else {
      this.isShowFilter2 = false;
      this.IsShowDDL2 = false;
    }
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

  getAllTaskTypeMaster() {
    
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.getAllTask).subscribe(
      (response) => {
        this.AllTaskTypeMaster = response;
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
  getTeamTaskTransactionWithPagination(currentPage1: number) {
    
    var date = new Date();
    if (
      this.supportingInchargeFromDateFilter == null ||
      this.supportingInchargeToDateFilter == null
    ) {
      this.supportingInchargeFromDateFilter = new Date(
        date.getFullYear(),
        date.getMonth(),
        1
      );
      this.supportingInchargeToDateFilter = date;
    } else if (
      this.supportingInchargeFromDateFilter != null ||
      this.supportingInchargeToDateFilter != null
    ) {
      this.supportingInchargeFromDateFilter;
      this.supportingInchargeToDateFilter;
    } else this.supportingInchargeToDateFilter = date;
    this.loading = true;
    let query = {
      currentPage1: currentPage1,
      ParentUserId: this.parentuserid,
      skiprow1: this.skiprow1,
      pagesize1: this.pageSize1,
      searchtext1: this.filterMyTeamTask,
      projectId: this.selectedProject,
      assignedById: this.selectedUserBy,
      assignedTo: this.selectedUserTo,
      taskTypeId: this.selectedTask,
      status: this.selectedStatus,
      fromDate: this.supportingInchargeFromDateFilter,
      toDate: this.supportingInchargeToDateFilter,
    };
    this.loading = true;
    // if(this.IsMyTeam = false){
    //   return;
    // }
    this.KemarService.get<any>(
      null,
      ServiceUrl.CoordinatingTeamTask,
      query
    ).subscribe(
      (response) => {
        this.AllMyTeamTaskData = response;
        if (response?.length > 0) {
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

  SearchForMyTeamTask() {
    this.currentPage1 = 1;
    this.getTeamTaskTransactionWithPagination(1);
  }

  DownloadTeamTaskTransaction() {
    
    var date = new Date();
    if (
      this.supportingInchargeFromDateFilter == null ||
      this.supportingInchargeToDateFilter == null
    ) {
      this.supportingInchargeFromDateFilter = new Date(
        date.getFullYear(),
        date.getMonth(),
        1
      );
      this.supportingInchargeToDateFilter = date;
    } else if (
      this.supportingInchargeFromDateFilter != null ||
      this.supportingInchargeToDateFilter != null
    ) {
      this.supportingInchargeFromDateFilter;
      this.supportingInchargeToDateFilter;
    } else this.supportingInchargeToDateFilter = date;
    this.loading = true;
    let query = {
      searchtext1: this.filterMyTeamTask,
      projectId: this.selectedProject,
      assignedById: this.selectedUserBy,
      assignedTo: this.selectedUserTo,
      taskTypeId: this.selectedTask,
      status: this.selectedStatus,
      fromDate: this.supportingInchargeFromDateFilter,
      toDate: this.supportingInchargeToDateFilter,
    };
    this.loading = true;
    // if(this.IsMyTeam = false){
    //   return;
    // }
    this.KemarService.get<any>(
      null,
      ServiceUrl.CoordinatingTeamTaskToDownload,
      query
    ).subscribe(
      (response) => {
        let dataForExcel: unknown[][] = [];

        if (response == undefined || response == null || response?.length == 0) {
          // Swal.fire('','Nothing to download','warning');
          return;
        }
        response.forEach((row: any) => {
          dataForExcel.push(Object.values(row));
        });

        let insideData = {
          title: 'Coordating Team Task Report',
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
}
