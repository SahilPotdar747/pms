import { Component, OnInit } from '@angular/core';
import { KemarServiceService } from '../service/kemar-service.service'; 
import { ServiceUrl } from '../service/service-url.service'; 
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { formatDate } from '@angular/common';
import { MessageService } from 'primeng/api';
import { ExcelServiceService } from '../service/excel-service.service'; 
import { DataService } from 'src/app/service/data.service';
import { Router } from '@angular/router';
import { bottom } from '@popperjs/core';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  position: string = 'right';
  loading =false;
  activeProject=0;
  newTask=0;
  wipTask=0;
  totalEmp = 0;
  selfNewTask = 0;
  PendingTask = 0;
  OverdueTask = 0;
  UnAssignTask = 0;
  departmentStatus:any;
  departmentStatusChart:any;
  pieChartData:any;
  departmentIdForPieChart=0;
  deptMaster:any;
  userRole = '';
  userIdForPieChart=0;
  userMaster:any;
  
  IsMyTeam: boolean = false;
  departmentOptions: any;
  pieOptions: any;



  constructor(
    private excelService: ExcelServiceService,
    private kemarService: KemarServiceService,
    private dataService: DataService,
    private fb: FormBuilder,
    private router: Router,
    private message: MessageService
  ) {
    this.kemarService.isLoggedIn$ = true;
  }

  ngOnInit(): void {
    this.getAllProjectMaster();
    this.getAllDept();
    this.departmentIdForPieChart = this.dataService.getUserDepartment();
    this.userIdForPieChart = this.dataService.getUserId();
    this.getAllAssignToUsers(this.departmentIdForPieChart);
    this.userRole = this.dataService.getUserRole();
    this.checkIHaveTeam();
    this.PieChartData();
  }
  

  
  getAllDept() {
    this.loading = true;
    this.kemarService.get<any>(null,ServiceUrl.getActiveDepartment).subscribe(
      (response) => {
        this.deptMaster = response;
        this.loading = false;
      },
      (r) => {
        this.kemarService.ShowMessage('error', 'Error', r.error.errorMessage == null ? r.error.message : r.error.errorMessage);
        this.loading = false;
      }
    );
  }

  getAllAssignToUsers(depertment: any) {
    var query = {
      departmentId: depertment
    };
    this.kemarService.get<any>(null,ServiceUrl.getAllUserbyDepartment,query).subscribe(
      (response) => {
        this.userMaster = response;
        var all = {
          id:0,
          firstName:'All',
          //lastName:''
        }
        this.userMaster.unshift(all);
        // this.departmentIdForPieChart=0;
  },
      (r) => {
  }
    );
    this.PieChartData();
  }

  getAllProjectMaster() {
    this.loading = true;
    this.kemarService.get<any>(null, ServiceUrl.getDashboardData).subscribe(
      (response) => {
        this.activeProject = response[0]?.activeProject;
        this.newTask = response[0]?.newTask;
        this.wipTask = response[0]?.wipTask;
        this.totalEmp = response[0]?.totalEmployee;
        this.departmentStatus = response[0]?.departmentTaskStatuses;
        this.selfNewTask = response[0]?.selfNewTask;
        this.PendingTask = response[0]?.pendingTask;
        this.OverdueTask = response[0]?.overdueTask;
        this.UnAssignTask = response[0]?.unAssigned;
        if(this.departmentStatus?.length > 0 ){
          this.BarChartofDepartmentStatus();
        }
        this.loading = false;
      },
      (r) => {
        this.loading = false;
      }
    );
  }

  checkIHaveTeam() {
    this.loading = true;
    this.kemarService.get<any>(null, ServiceUrl.CheckIHaveTeam).subscribe(
      (response) => {
        this.IsMyTeam = response;
        this.loading = false;
      },
      (r) => {
        this.loading = false;
        this.IsMyTeam = false;
        if (r.status == '401' && r.statusText == 'Unauthorized') {
          this.kemarService.ShowMessage(
            'error',
            r.statusText,
            'Please Login again'
          );
        }
      }
    );
  }

  BarChartofDepartmentStatus() {
    let chart1Lables = this.departmentStatus.map((x: { departmentName: any; }) => x.departmentName);
    let chart1NewTask = this.departmentStatus.map((x: { newTask: any; }) => x.newTask);
    let chartWIPTask = this.departmentStatus.map((x: { wipTask: any; }) => x.wipTask);
    let charUnAssignedTask = this.departmentStatus.map((x: { unAssigned: any; }) => x.unAssigned);
    let charPendinTask = this.departmentStatus.map((x: { pending: any; }) => x.pending);
    let charOverdueTask = this.departmentStatus.map((x: { overdue: any; }) => x.overdue);
    this.departmentStatusChart = {
      // labels: chart1Lables,
      // labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
      datasets: [{ label: 'New Task', backgroundColor: '#3597e6', data: chart1NewTask },
      { label: 'WIP Task', backgroundColor: '#119132', data: chartWIPTask },
      { label: 'UnAssign', backgroundColor: '#929493', data: charUnAssignedTask },
      { label: 'Pending', backgroundColor: '#FFA726', data: charPendinTask },
      { label: 'Overdue', backgroundColor: '#e03b26', data: charOverdueTask }]
    };
    this.departmentOptions = {
      plugins: {
        legend: {
          position: bottom ,
            labels: {
                boxWidth: 10,
                // color: 'white'
            }
        }
    },
    // scales: {
    //     x: {
    //       type: 'linear',
    //         display: true,
    //         position: 'left',
    //         ticks: {
    //             color: '#888888'
    //         },
    //         grid: {
    //             color: '#888888'
    //         }
    //     },
    //     y: {
    //         type: 'linear',
    //         display: true,
    //         position: 'left',
    //         ticks: {
    //             min: 0,
    //             max: 100,
    //             color: '#888888'
    //         },
    //         grid: {
    //             color: '#888888'
    //         }
    //     }
    // }
    };

  }

  PieChartData(){
    var query = {
      departmentId: this.departmentIdForPieChart,
      userId: this.userIdForPieChart
    }
    this.kemarService.get<any>(null, ServiceUrl.getDashboardPiaData, query).subscribe(
      (response) => {
        this.pieChartData = response;
        // let data = response.map();
        this.pieChartData = {
          labels: ['New Task','Work In Progress','UnAssigned','Pending','Overdue'],
          datasets: [
              {
                  data: [response[0]?.newTask, response[0]?.wipTask, response[0]?.unAssigned, response[0]?.pending, response[0]?.overdue],
                  backgroundColor: [
                      "#3597e6",
                      "#119132",
                      "#929493",
                      "#FFA726",
                      "#e03b26"
                  ]
              }
          ],
          backgroundColor: '#FFB1C1',
      };
      this.pieOptions = {
        plugins: {
          title: {
            display: true,
            // text: 'My Title',
        },
          legend: {
            position: 'bottom',
              labels: {
                boxWidth: 10,
                render: 'percentage',
                font: {size: 12},
                // color:'white',
                // precision: 2,
                // fontStyle: 'italic',
                // usePointStyle: true,
            }
          }
      },
    };
    },
      (r) => {
        this.loading = false;
      }
    );

  }

  CardClick(cardName: string) {
    switch (cardName) {
      case 'Project':
        this.router.navigate(['/project']);
        break;
      case 'Task':
        this.router.navigate(['/task']);
        break;
      case 'NewTask':
        this.router.navigate(['/newTask']);
        break;
      case 'TotalEmp':
        this.router.navigate(['/alluserreport']);
        break;
      case 'UnAssign':
        this.router.navigate(['/newTask']);
        break;
    }
  }



}
