import { Component, OnInit } from '@angular/core';
import { faLaptopHouse, faSearch, faTimes, faDownload } from '@fortawesome/free-solid-svg-icons';
import { KemarServiceService } from 'src/app/service/kemar-service.service';
import { ServiceUrl } from 'src/app/service/service-url.service';
import { StatusDataService } from 'src/app/service/status-data.service';
import { ExcelServiceService } from 'src/app/service/excel-service.service';


@Component({
  selector: 'app-project-wise-report',
  templateUrl: './project-wise-report.component.html',
  styleUrls: ['./project-wise-report.component.scss']
})
export class ProjectWiseReportComponent {
  faSearch = faSearch;
  faTimes = faTimes;
  faDownload = faDownload;
  loading = false;
  ProjectWiseTask: any;
  totalCountOfProjectWise: any;
  pageSize = 15;
  currentPage = 1;
  skiprow: number = 0;
  filterReport: any;
  selectedProject: number = 0;
  selectedUserBy: number = 0;
  selectedTask: number = 0;
  selectedUserTo: number = 0;
  projectReportStatusFilter: any;
  reportFromDateFilter: any;
  reportToDateFilter: any;
  reportCountFromDateFilter: any;
  reportcountToDateFilter: any;
  GetProjectWiseTask: any;
  GetProjectWiseTaskcount: any;
  projectWiseTaskCount: any;
  IsShowDDL1 = false;
  isShowFilter1 = true;
  filterProjectWise = '';
  AllTaskTypeMaster: any;
  AllProjectMaster: any;
  AllAssignToUsers: any;
  GetAllAssignedBy: any;
  GetAllAssignedTo: any;
  taskStatus: any;

  constructor(
    private KemarService: KemarServiceService,
    private statusData: StatusDataService,
    private excel: ExcelServiceService
  ) {
    this.KemarService.isLoggedIn$ = true;
  }

  ngOnInit(): void {
    this.taskStatus = this.statusData.TaskStatusForFilter();
    this.getprojectWiseTask();
    this.getProjectWiseTaskCountReport();
    this.getAllAssignedBy();
    this.getAllAssignedTo();
    this.getAllProjectMaster();
    this.getAllTaskTypeMaster();
    var date = new Date();
    this.reportFromDateFilter = new Date(
      date.getFullYear(),
      date.getMonth(),
      1
    );
    this.reportToDateFilter = date;
  }

  CloseFilter_Tab2() {
    this.isShowFilter1 = true;
    this.IsShowDDL1 = false;
  }

  pagechange(event: any) {
    
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getProjectWiseTaskCountReport();
  }

  pagechange1(event: any) {
    
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getprojectWiseTask();
  }

  Search() {
    
    this.getprojectWiseTask();
  }

  OnResetDDL_Tab1() {
    //this.Reset();
    this.selectedUserBy = 0;
    this.filterProjectWise = '';
    this.selectedProject = 0;
    this.selectedTask = 0;
    this.getprojectWiseTask();  
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

  getProjectWiseTaskCountReport() {
    
    var date = new Date();
    if (
      this.reportCountFromDateFilter == null ||
      this.reportcountToDateFilter == null
    ) {
      this.reportCountFromDateFilter = new Date(
        date.getFullYear(),
        date.getMonth(),
        1
      );
      this.reportcountToDateFilter = date;
    } else if (
      this.reportCountFromDateFilter != null ||
      this.reportcountToDateFilter != null
    ) {
      this.reportCountFromDateFilter;
      this.reportcountToDateFilter;
    } else this.reportcountToDateFilter = date;
    this.loading = true;
    let query = {
      fromDate: this.reportCountFromDateFilter,
      toDate: this.reportcountToDateFilter,
      pageSize: this.pageSize,
      skipRow: this.skiprow,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.GetProjectWiseTaskCountReport,
      query
    ).subscribe(
      (response) => {
        this.GetProjectWiseTaskcount = response;
        if (response.length > 0) {
          this.projectWiseTaskCount = response[0].totalRecord;
        } else {
          this.projectWiseTaskCount = 0;
        }
      },
      (r) => {
        this.KemarService.ShowMessage('error', 'Error', r.error.errorMessage);
      }
    );
    this.loading = false;
  }

  getprojectWiseTask() {
    
    var date = new Date();
    if (this.reportFromDateFilter == null || this.reportToDateFilter == null) {
      this.reportFromDateFilter = new Date(
        date.getFullYear(),
        date.getMonth(),
        1
      );
      this.reportToDateFilter = date;
    } else if (
      this.reportFromDateFilter != null ||
      this.reportToDateFilter != null
    ) {
      this.reportFromDateFilter;
      this.reportToDateFilter;
    } else this.reportToDateFilter = date;
    this.loading = true;
    let query = {
      skipRow: this.skiprow,
      pageSize: this.pageSize,
      searchtext: this.filterProjectWise,
      projectId: this.selectedProject,
      assignedById: this.selectedUserBy,
      assignedTo: this.selectedUserTo,
      taskTypeId: this.selectedTask,
      fromDate: this.reportFromDateFilter,
      toDate: this.reportToDateFilter,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.GetProjectWiseTask,
      query
    ).subscribe(
      (response) => {
        this.GetProjectWiseTask = response;
        if (response.length > 0) {
          this.ProjectWiseTask = response[0].totalRecord;
        } else {
          this.ProjectWiseTask = 0;
        }
      },
      (r) => {
        this.KemarService.ShowMessage('error', 'Error', r.error.errorMessage);
      }
    );
    this.loading = false;
  }

  DownloadCountReport() {
    var date = new Date();
    if (
      this.reportCountFromDateFilter == null ||
      this.reportcountToDateFilter == null
    ) {
      this.reportCountFromDateFilter = new Date(
        date.getFullYear(),
        date.getMonth(),
        1
      );
      this.reportcountToDateFilter = date;
    } else if (
      this.reportCountFromDateFilter != null ||
      this.reportcountToDateFilter != null
    ) {
      this.reportCountFromDateFilter;
      this.reportcountToDateFilter;
    } else this.reportcountToDateFilter = date;
    this.loading = true;
    let query = {
      fromDate: this.reportCountFromDateFilter,
      toDate: this.reportcountToDateFilter,
      pageSize: this.pageSize,
      skipRow: this.skiprow,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.GetProjectWiseTaskCountReportToDownload,
      query
    ).subscribe(
      (response) => {
        let dataForExcel: unknown[][] = [];

        if (response.length == 0) {
          // Swal.fire('','Nothing to download','warning');
          return;
        }
        response.forEach((row: any) => {
          dataForExcel.push(Object.values(row));
        });

        let insideData = {
          title: 'Project Wise Task Count Summary Report',
          data: dataForExcel,
          headers: Object.keys(response[0]),
        };
        this.excel.exportExcelForProjectWiseCountReport(insideData);
      },
      (r) => {
        this.KemarService.ShowMessage('error', 'Error', r.error.errorMessage);
      }
    );
    this.loading = false;
  }

  DownloadProjectWiseTaskReport() {
    
    var date = new Date();
    if (this.reportFromDateFilter == null || this.reportToDateFilter == null) {
      this.reportFromDateFilter = new Date(
        date.getFullYear(),
        date.getMonth(),
        1
      );
      this.reportToDateFilter = date;
    } else if (
      this.reportFromDateFilter != null ||
      this.reportToDateFilter != null
    ) {
      this.reportFromDateFilter;
      this.reportToDateFilter;
    } else this.reportToDateFilter = date;
    this.loading = true;
    let query = {
      searchtext: this.filterProjectWise,
      projectId: this.selectedProject,
      assignedById: this.selectedUserBy,
      assignedTo: this.selectedUserTo,
      taskTypeId: this.selectedTask,
      fromDate: this.reportFromDateFilter,
      toDate: this.reportToDateFilter,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.GetProjectWiseTaskToDownload,
      query
    ).subscribe(
      (response) => {
        let dataForExcel: unknown[][] = [];

        if (response.length == 0) {
          // Swal.fire('','Nothing to download','warning');
          return;
        }
        response.forEach((row: any) => {
          dataForExcel.push(Object.values(row));
        });

        let insideData = {
          title: 'Project Wise Task Summary Report',
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
