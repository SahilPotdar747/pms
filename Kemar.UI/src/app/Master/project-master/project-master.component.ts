import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Table } from 'exceljs';
import { MessageService } from 'primeng/api';
import { ExcelServiceService } from 'src/app/service/excel-service.service';
import { KemarServiceService } from 'src/app/service/kemar-service.service';
import { ServiceUrl } from 'src/app/service/service-url.service';
import { StatusDataService } from 'src/app/service/status-data.service';
import { ProjectMasterResponse, UserRegistration } from 'src/app/service/user-model.service';
import { faLeaf, faSearch } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-project-master',
  templateUrl: './project-master.component.html',
  styleUrls: ['./project-master.component.scss']
})
export class ProjectMasterComponent {
  public userRegistration: UserRegistration = new UserRegistration();
  public ProjectMaster: ProjectMasterResponse = new ProjectMasterResponse();
  loading = false;
  @ViewChild('dt') dt: Table | undefined;
  title: string = "'Add Project'";
  projectform! : FormGroup;
  loginUser : string = '';
  submitted!: boolean;
  projectMaster: any;
  ManagerList: any;
  display: boolean = false;
  Save!: string;
  spinner: boolean = false;
  faSearch = faSearch;
  $index=0;
  projectStatusFilter = '';
  projectStatusResponse : any;
  isRemark:boolean= false;

  //sorting
  public filter : string = '';
  filterProjectList:string = '';
  skiprow:number=0;
  pageSize = 15;
  currentPage = 1;
  totalRecords : any;
  last:any;
  projectStatus: any;
  status: any;

  pagechange(event: any) {
    
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getAllProjectWithPagination(1);
  }

  constructor(private KemarService: KemarServiceService,
    private excelService: ExcelServiceService,
    private statusData: StatusDataService,
    private fb: FormBuilder,
    private message : MessageService
    ) {
    this.KemarService.isLoggedIn$ = true;
   }

  ngOnInit(): void {
    this.projectStatus = this.statusData.getProjectStatus();
    this.status = this.statusData.projectStatus();
    this.projectStatusFilter = '';
    this.getAllProject();
    this.Reset();
    this.getAllProjectWithPagination(1);
    this.getManagerDDL();
  }

  Reset() {
    const nonWhitespaceRegExp: RegExp = new RegExp("\\S");
    this.projectform = this.fb.group({
      projectId: [0,[Validators.required]],
      projectName: [
        '',
         [
          Validators.required, 
          Validators.minLength(3),
          //Validators.pattern('[a-z A-Z-]*'), 
          //Validators.pattern('[^-\\s][A-Za-z0-9()*%!_@./#&+:=-\\s]*'),
          Validators.maxLength(50),
          this.noWhitespaceValidator
          //Validators.pattern(nonWhitespaceRegExp)
        ]],
      description: ['', [Validators.maxLength(1000)]],
      remark: [''],
      //id: [0, [Validators.required]],
      startDate: ['', [Validators.required]],
      endDate: ['', [Validators.required]],
      status: ['', [Validators.required]],
      isActive: [true, [Validators.required,]],
      managerId:['',Validators.required]

    });
  }

  onReset() {
    this.submitted = false;
    this.projectform.reset();
    this.Reset();
  } 

  ShowMessage(messageType: string,title: string,message:string) {
    this.message.add({severity:messageType, summary:title, detail: message});
}

get f(): { [key: string]: AbstractControl } {
  return this.projectform.controls;
}

// applyFilterGlobal($event: any, stringVal: any) {
//   this.dt!.filterGlobal(($event.target as HTMLInputElement).value, stringVal);
// }

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

getAllProject() {   // this method not using for list
  this.loading = true;
  this.KemarService.get<any>(null, ServiceUrl.getAllProject)
    .subscribe(
      response => {
        this.projectMaster = response;
      },
      r => {
        this.ShowMessage('error','Error',r.error.errorMessage);
      });
      this.loading = false;
}

// Search(){
//   this.skiprow = 0;
//   this.currentPage = 1;
//   this.searchProjectWithPagination(1);
// }

searchProjectWithPagination(currentPage: any) {
  
  var query = {
    currentPage: currentPage,
    status: this.projectStatusFilter,
    skipRow:this.skiprow,
    rowSize: this.pageSize,
    searchtext: this.filterProjectList,
  };
  this.KemarService.get<any>(null, ServiceUrl.getAllProjectWithPagination, query)
    .subscribe(
      response => {
        this.projectMaster = response;
        if(response?.length > 0){
          this.totalRecords = response[0].totalRecord;
        }
        else{
          this.totalRecords = 0;
        }
        this.currentPage = currentPage;
        this.loading = false;
      },
      r => {
        this.ShowMessage('error','Error',r.error.errorMessage);
        this.loading = false;
      });

}


getAllProjectWithPagination(currentPage: any) {
  
  var query = {
    currentPage: currentPage,
    status: this.projectStatusFilter,
    skipRow:this.skiprow,
    rowSize: this.pageSize,
    searchtext: this.filterProjectList,
  };
  this.loading = true;
  this.KemarService.get<any>(null, ServiceUrl.getAllProjectWithPagination, query)
    .subscribe(
      response => {
        this.projectMaster = response;
        if(response?.length > 0){
          this.totalRecords = response[0].totalRecord;
        }
        else{
          this.totalRecords = 0;
        }
        this.currentPage = currentPage;
        this.loading = false;
      },
      r => {
        this.ShowMessage('error','Error',r.error.errorMessage);
        this.loading = false;
      });

}

loadLazy(event: any) {
  const count = (event.first + 10) / 15;
  console.log(event);
  //this.sort(event.sortField,event.sortOrder);
}

// sort(fieldName:string, order:number){
//   console.log(fieldName);
//   console.log(order)
//   this.projectMaster.sort(()=>{
//     const val1=row1[fieldName];
//     const val2=row2[fieldName];
//     if(val1===val2){
//       return 0;
//     }
//     let result=-1;
//     if(val1>val2){
//       result=1;
//     }
//     if(order<0){
//       result=-result;
//     }
//     return result;
//   });
// }

getManagerDDL(){
  
  this.KemarService.get<UserRegistration>(null, ServiceUrl.getManagerDDL)
    .subscribe(
      response => {
        this.ManagerList = response;
      },
      r => {
        this.ShowMessage('error','Error',r.error.errorMessage);
      });
      this.loading = false;
}

  open(){
    
    this.submitted = false;
    // this.projectform.reset();
    this.isRemark = false;
    this.title = "Add Project";
    this.Save = "Add";
    //this.projectform.controls['id'].setValue(0);
    this.projectform.reset();
    this.Reset();
    // this.projectform.controls['projectName'].setValue('');
    //this.projectform.controls['description'].setValue('');
    // this.projectform.controls['remark'].setValue('');
    // this.projectform.controls['managerId'].setValue(0);
    // this.projectform.controls['startDate'].setValue(0);
    // this.projectform.controls['endDate'].setValue(0);
    // this.projectform.controls['status'].setValue('');
    //this.projectform.controls['isActive'].setValue(true);
    this.display = true;
  }

  editProject(project: any) {
    this.submitted = false;
    this.projectform.reset();
    this.Reset();
    this.title = "Update Project";
    this.isRemark = true;
    this.projectform.patchValue(project);
    this.projectform.controls['projectId'].setValue(project.projectId);
    this.projectform.controls['startDate'].setValue(project.startDate.split('T')[0]);
    this.projectform.controls['endDate'].setValue(project.endDate.split('T')[0]);
    this.Save = "Update";
    this.display = true;
  }

  registerProject() {
    
    this.submitted = true;
    if (this.projectform.invalid) {
      return;
    }
    this.loading = true;
    let body = this.projectform.value;
    let startDate = this.projectform.controls['startDate'];
    let endDate = this.projectform.controls['endDate'];
    if (startDate.value > endDate.value) {
      this.KemarService.ShowMessage(
        'error',
        'Error',
        "End date should be greater than start date"
      );
      this.loading = false;
      return;
    }
    this.KemarService.postPatch<any>(ServiceUrl.registerProject, body)
      .subscribe(
        res => {
          this.ShowMessage('success','Success',res.responseMessage);
          this.getAllProjectWithPagination(1);
        },
        r => {
          this.ShowMessage('error','Error',r.error.errorMessage);
        });
        this.display = false;
        this.loading = false;
  }

}
