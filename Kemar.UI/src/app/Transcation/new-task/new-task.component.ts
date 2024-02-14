import { Component, OnInit, ViewChild } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { variationPlacements } from '@popperjs/core';
import { Table } from 'exceljs';
import { MessageService } from 'primeng/api';
import { ExcelServiceService } from 'src/app/service/excel-service.service';
import { KemarServiceService } from 'src/app/service/kemar-service.service';
import { ServiceUrl } from 'src/app/service/service-url.service';
import { StatusDataService } from '../../service/status-data.service';
import { DataService } from 'src/app/service/data.service';
import { faSearch, faTimes, faUserInjured } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-new-task',
  templateUrl: './new-task.component.html',
  styleUrls: ['./new-task.component.scss']
})
export class NewTaskComponent {
  faSearch = faSearch;
  faTimes=faTimes;
  TaskForm!: FormGroup;
  @ViewChild('dt1') dt1: Table | undefined;
  selectedDepartment: number = 0;
  loading = false;
  deptMaster: any;
  unAssignTask: any;
  display = false;
  submitted = false;
  title = 'Add New Task';
  AllProjectMaster: any;
  projectMaster: any;
  AllTaskTypeMaster: any;
  taskTypeMaster: any;
  AllAssignToUsers: any;
  AllPriority: any;
  AllStatus: any;
  SaveButtonTitle = 'Add';
  selectedProject: number = 0;
  selectedTaskType: number = 0;
  isShowFilter: boolean = true;
  IsShowDDL: boolean = false;
  star1 = false;
  star2 = false;

  //sorting MyTaskPage
  totalRecords: any;
  pageSize = 15;
  currentPage = 1;
  skiprow = 0;
  filterMyTask = '';

  constructor(
    private KemarService: KemarServiceService,
    private fb: FormBuilder,
    private statusData: StatusDataService,
    private dataService: DataService
  ) {
    this.KemarService.isLoggedIn$ = true;
  }

  ngOnInit(): void {
    this.Reset();
    this.selectedDepartment = this.dataService.getUserDepartment();
    this.AllPriority = this.statusData.getTaskPriority();
    this.AllStatus = this.statusData.getTaskStatus();
    this.getAllUnAssignTaskWithPagination(1);
    this.getAllDept();
    this.getAllProjectMaster();
    this.getProjectMaster();
    this.getAllTaskTypeMaster(this.selectedDepartment);
    this.getTaskTypeMaster(this.selectedDepartment);
    this.getAllAssignToUsers(this.selectedDepartment);
  }

  Reset() {
    const nonWhitespaceRegExp: RegExp = new RegExp("\\S");
    this.TaskForm = this.fb.group({
      taskId: [0, [Validators.required]],
      title: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(100),
          //Validators.pattern('[a-z A-Z-]*'),
          //Validators.pattern('[^-\\s][A-Za-z0-9()*%!_@./#&+:=-\\s]*'),
          this.noWhitespaceValidator,
          //Validators.pattern(nonWhitespaceRegExp),
        ],
      ],
      projectId: ['', [Validators.required]],
      taskTypeId: ['', [Validators.required]],
      description: ['', [Validators.maxLength(1000)]],
      priority: ['', [Validators.required]],
      assignedTo: [''],
      assignedById: ['0', [Validators.required]],
      departmentId: ['', [Validators.required]],
      assignedBy: [''],
      assignedDate: [''],
      exceptedStartDate: [''],
      exceptedEndDate: [''],
      status: ['', [Validators.required]],
      isActive: [true],
    });
    this.star1 = false;
    this.star2 = false;
  }

  OnReset(){
    this.submitted = false;
    this.TaskForm.reset();
    this.Reset();
  }

  get f(): { [key: string]: AbstractControl } {
    return this.TaskForm.controls;
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
      this.star1 = true;
      this.star2 = true;
    } else {
      this.TaskForm.controls['exceptedStartDate'].clearValidators();
      this.TaskForm.controls['exceptedEndDate'].clearValidators();
      this.TaskForm.controls['exceptedStartDate'].updateValueAndValidity();
      this.TaskForm.controls['exceptedEndDate'].updateValueAndValidity();
      this.star1 = false;
      this.star2 = false;
    }
  }

  CloseFilter(){
    this.isShowFilter = true;
    this.IsShowDDL = false;
  }

  OnResetDDL() {
    this.filterMyTask = '';
    this.selectedTaskType = 0;
    this.selectedProject = 0;
    this.selectedDepartment = this.dataService.getUserDepartment();
    this.getAllUnAssignTaskWithPagination(1);
  }

  onFilter() {
    
    if ((this.isShowFilter = true)) {
      this.IsShowDDL = true;
      this.isShowFilter = false;
    } else {
      this.isShowFilter = false;
      this.IsShowDDL = false;
    }
  }
  getAllDept() {
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

  getAllUnAssignTaskWithPagination(currentPage: any) {
    
    this.loading = true;
    var query = {
      //currentPage:currentPage,
      departmentId: this.selectedDepartment,
      skiprow: this.skiprow,
      pagesize: this.pageSize,
      searchtext: this.filterMyTask,
      projectId: this.selectedProject,
      taskTypeId: this.selectedTaskType,
    };

    this.KemarService.get<any>(
      null,
      ServiceUrl.getAllUnAssignedTask,
      query
    ).subscribe(
      (response) => {
        this.unAssignTask = response;
        if (response?.length > 0) {
          this.totalRecords = response[0].totalRecord;
        } else {
          this.totalRecords = 0;
        }

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

  Search() {
    
    this.currentPage = 1;
    this.getAllUnAssignTaskWithPagination(this.currentPage);
  }

  pagechange(event: any) {
    
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getAllUnAssignTaskWithPagination(1);
  }

  updateTask() {
    
    this.submitted = true;
    if (this.TaskForm.invalid) {
      return;
    }
    let startDate = this.TaskForm.controls['exceptedStartDate'];
    let endDate = this.TaskForm.controls['exceptedEndDate'];
    if (startDate.value > endDate.value) {
      this.KemarService.ShowMessage(
        'error',
        'Error',
        'End date should be greater than start date'
      );
      this.loading = false;
      return;
    }
    this.loading = true;
    if (
      this.TaskForm.get('assignedTo')!.value == '' ||
      this.TaskForm.get('assignedTo')!.value == 0
    ) {
      this.TaskForm.controls['status'].setValue('UnAssigned');
    } else {
      this.TaskForm.controls['status'].setValue('New Task');
    }

    var body = this.TaskForm.value;

    this.KemarService.postPatch<any>(ServiceUrl.RegisterTask, body).subscribe(
      (res) => {
        this.display = false;
        this.loading = false;
        this.KemarService.ShowMessage(
          'success',
          'Success',
          res.responseMessage
        );
        this.getAllUnAssignTaskWithPagination(1);
      },
      (r) => {
        this.display = false;
        this.loading = false;
        this.KemarService.ShowMessage(
          'error',
          'Error',
          r.error.errorMessage == null ? r.error.message : r.error.errorMessage
        );
      }
    );
  }

  OpenNewTask() {
    this.TaskForm.controls['title'].enable();
    this.Reset();
    this.submitted = false;
    this.SaveButtonTitle = 'Add';
    this.TaskForm.controls['assignedTo'].setValue('');
    this.TaskForm.controls['departmentId'].setValue(this.selectedDepartment);
    this.TaskForm.controls['status'].setValue('UnAssigned');

    this.display = true;
  }

  editMyTask(task: any) {
    this.TaskForm.controls['title'].disable();
    this.submitted = false;
    this.SaveButtonTitle = 'Update';
    this.Reset();
    this.TaskForm.patchValue(task);
    this.TaskForm.controls['exceptedStartDate'].setValue(
      task.exceptedStartDate?.replace('T', ' ')
    );
    this.TaskForm.controls['exceptedEndDate'].setValue(
      task.exceptedEndDate?.replace('T', ' ')
    );
    this.getAllAssignToUsers(task.departmentId);
    this.display = true;
  }

  // Have to Implement
  getAllTaskTypeMaster(depertment: any) {
    this.loading = true;
    this.TaskForm.controls['taskTypeId'].disable();
    var query = {
      departmentId: depertment,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.getAllTaskTypeDepartmentWise,
      query
    ).subscribe(
      (response) => {
        this.AllTaskTypeMaster = response;
        var all = {
          taskId: 0,
          taskName: 'Task Type',
        };
        this.AllTaskTypeMaster.unshift(all);
        this.TaskForm.controls['taskTypeId'].enable();
        this.TaskForm.controls['taskTypeId'].setValue('');
        this.loading = false;
      },
      (r) => {
        this.TaskForm.controls['taskTypeId'].enable();
        this.TaskForm.controls['taskTypeId'].setValue('');
        this.loading = false;
      }
    );
  }

  getTaskTypeMaster(depertment: any) {
    this.loading = true;
    this.TaskForm.controls['taskTypeId'].disable();
    var query = {
      departmentId: depertment,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.getAllTaskTypeDepartmentWise,
      query
    ).subscribe(
      (response) => {
        this.taskTypeMaster = response;
        this.TaskForm.controls['taskTypeId'].enable();
        this.TaskForm.controls['taskTypeId'].setValue('');
        this.loading = false;
      },
      (r) => {
        this.TaskForm.controls['taskTypeId'].enable();
        this.TaskForm.controls['taskTypeId'].setValue('');
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
          projectName: 'Projects',
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

  ChangeDepartment($event: any) {
    this.getAllAssignToUsers(this.TaskForm.get('departmentId')!.value);
    this.getAllTaskTypeMaster(this.TaskForm.get('departmentId')!.value);
  }

  getAllAssignToUsers(depertment: any) {
    this.loading = true;
    this.TaskForm.controls['assignedTo'].disable();
    var query = {
      departmentId: depertment,
    };
    this.KemarService.get<any>(
      null,
      ServiceUrl.getAllUserbyDepartment,
      query
    ).subscribe(
      (response) => {
        this.AllAssignToUsers = response;
        this.loading = false;
        this.TaskForm.controls['assignedTo'].enable();
        this.TaskForm.controls['assignedTo'].setValue('');
      },
      (r) => {
        this.loading = false;
        this.TaskForm.controls['assignedTo'].enable();
        this.TaskForm.controls['assignedTo'].setValue('');
      }
    );
  }
}
