import { Component, OnInit, ViewChild } from '@angular/core';
import { KemarServiceService }  from '../../service/kemar-service.service';
import { ServiceUrl } from '../../service/service-url.service';
import { Table } from 'primeng/table';
import { ExcelServiceService } from '../../service/excel-service.service';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
@Component({
  selector: 'app-role-management',
  templateUrl: './role-management.component.html',
  styleUrls: ['./role-management.component.scss']
})
export class RoleManagementComponent implements OnInit {
  roleMaster!: any[];
  @ViewChild('dt') dt: Table | undefined;
  roleform! : FormGroup;
  loginUser : string = '';
  submitted!: boolean;
  ShowActive: boolean = false;
  display: boolean = false;
  title: string = 'Add Role';
  Save!: string;
  spinner: boolean = false;
  loading = false;
  $index=0;

  //sorting
  last: any;
  //public filter : string = '';
  filterRoleList : string = '';
  pageSize = 15;
  currentPage = 1;
  totalRecords = 10;
  skiprow: number = 15;
  filter = '';
  statusRowValue = '';

  pagechange(event: any) {
    
    this.skiprow = event.first;
    this.pageSize = event.rows;
    this.getAllRolesWithPagination(1);

  }

  constructor(private KemarService : KemarServiceService,
    private fb: FormBuilder,
    private message : MessageService) {
    this.KemarService.isLoggedIn$ = true;
     }

  ngOnInit(): void {
    // this.getAllRoles();
    this.Reset();
    this.getAllRolesWithPagination(1);
  }

  Reset() {
    this.roleform = this.fb.group({
      roleId:[0],
      roleName: ['', 
      [
        Validators.required, 
        Validators.minLength(3), 
        Validators.maxLength(30),
        this.noWhitespaceValidator,
        //Validators.pattern('[^-\\s][A-Za-z0-9()*%!_@./#&+:=-\\s]*')
      ]
    ],
      roleGroup: ['', 
      [
        Validators.required,
        Validators.minLength(3), 
        Validators.maxLength(100),
        this.noWhitespaceValidator,
        //Validators.pattern('[^-\\s][A-Za-z0-9()*%!_@./#&+:=-\\s]*')
      ]
    ],
      isActive: ['', [Validators.required,]],
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

  onReset(): void {
    this.submitted = false;
    this.Reset();
  }

  ShowMessage(messageType: string,title: string,message:string) {
    this.message.add({severity:messageType, summary:title, detail: message});
}

  get f(): { [key: string]: AbstractControl } {
    return this.roleform.controls;
  }


  getAllRoles() {
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.getAllRoles)
      .subscribe(
        response => {
          this.roleMaster = response;
        },
        r => {
          this.ShowMessage('error','Error',r.error.errorMessage);
        });
        this.loading = false;
  }

  // Search(){
  //   this.skiprow = 0;
  //   this.currentPage = 1;
  //   this.searchAllRolesWirhPagination(1);
  // }

  searchAllRolesWirhPagination(currentPage: any){
    
    var query = {
      currentPage: currentPage,
      //skipRow: this.skiprow,
      rowSize: this.pageSize,
      searchtext: this.filterRoleList,
    };
    this.KemarService.get<any>(null, ServiceUrl.getAllRolesWithPagination, query)
      .subscribe(
        response => {
          this.roleMaster = response;
          if(response?.length > 0){
            this.totalRecords = response[0].totalRecord;
          }
          else{
            this.totalRecords = 0;
          }
          this.currentPage = currentPage;
        },
        r => {
          this.ShowMessage('error','Error',r.error.errorMessage);
        });
        this.loading = false;
  }
  

  getAllRolesWithPagination(currentPage: any) {
    
    var query = {
      currentPage: currentPage,
      //skipRow: this.skiprow,
      rowSize: this.pageSize,
      searchtext: this.filterRoleList,
    };
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.getAllRolesWithPagination, query)
      .subscribe(
        response => {
          this.roleMaster = response;
          if(response?.length > 0){
            this.totalRecords = response[0].totalRecord;
          }
          else{
            this.totalRecords = 0;
          }
          this.currentPage = currentPage;
        },
        r => {
          this.ShowMessage('error','Error',r.error.errorMessage);
        });
        this.loading = false;
  }

  open() {
    this.submitted = false;
    this.roleform.reset();
    this.ShowActive = true;
    this.title = "Add Role";
    this.Save = "Add";
    this.roleform.controls['roleId'].setValue(0);
    this.roleform.controls['roleName'].setValue('');
    this.roleform.controls['isActive'].setValue(true);
    this.display = true;
  }

  editFilter(role: any) {
    this.submitted = false;
    this.ShowActive = true;
    this.roleform.reset();
    this.title = "Update Role";
    this.roleform.patchValue(role);
    this.Save = "Update";
    this.display = true;
  }

  registerRole() {
    
    this.submitted = true;
    if (this.roleform.invalid) {
      return;
    }
    this.loading = true;
    let body = this.roleform.value;
    this.KemarService.postPatch<any>(ServiceUrl.registerRole, body)
      .subscribe(
        res => {
          this.ShowMessage('success','Success',res.responseMessage);
          this.getAllRolesWithPagination(1);
        },
        r => {
          this.ShowMessage('error','Error',r.error.errorMessage);
        });
        this.display = false;
        this.loading = false;
  }

}
