import { Component, OnInit, ViewChild } from '@angular/core';
import { KemarServiceService } from '../../service/kemar-service.service';
import { ServiceUrl } from '../../service/service-url.service';
import { Table } from 'primeng/table';
import { ExcelServiceService } from '../../service/excel-service.service';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { AllUserRoleAccess, UserAccessManagerResponse, UserRoleAccessMapping } from 'src/app/service/user-model.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { faTimes } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: "app-user-role-mapping",
  templateUrl: "./user-role-mapping.component.html",
  styleUrls: ["./user-role-mapping.component.scss"],
})
export class UserRoleMappingComponent implements OnInit {
  userRoleMappingMaster!: any[];
  @ViewChild('dt2') dt2: Table | undefined;
  userRoleMappingform!: FormGroup;
  faTimes = faTimes;
  loginUser: string = '';
  submitted!: boolean;
  ShowActive: boolean = false;
  display: boolean = false;
  title: string = 'Add Role';
  Save: string = 'Update';
  spinner: boolean = false;
  loading = false;
  $index = 0;
  singleUserAccessMapping: any;
  allUserAccessMapping: any;
  roleMaster: any;
  singleUserAccessResponse: any;
  userAccessManagerResponse!: any;
  ReadChecked: Boolean= false;
  CreateChecked: Boolean= false;
  UpdateChecked: Boolean= false;
  DeleteChecked: Boolean= false;
  pageSize = 15;
  skipRow = 0;

  constructor(private KemarService: KemarServiceService,
    private excelService: ExcelServiceService,
    private fb: FormBuilder,
    private message: MessageService,
    private modalService: NgbModal) { }

  ngOnInit(): void {
    this.KemarService.isLoggedIn$ = true;
    this.getAllUserRoleMapping();
    this.getAllRoles();
    this.Reset();
    this.KemarService.isLoggedIn$ = true;
  }

  Reset() {
    this.userRoleMappingform = this.fb.group({
      roleName: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(30)]],
      isActive: ['', [Validators.required,]],

    });
  }

  ShowMessage(messageType: string, title: string, message: string) {
    this.message.add({ severity: messageType, summary: title, detail: message });
  }

  applyFilterGlobal($event: any, stringVal: any) {
    
    this.dt2!.filterGlobal(($event.target as HTMLInputElement).value, stringVal);
  }

  getAllUserRoleMapping() {
    
    this.loading = true;
    this.KemarService.get<AllUserRoleAccess>(null, ServiceUrl.getAllUserRoleAccessMapping)
      .subscribe(
        userAccessResponse => {
          this.allUserAccessMapping = userAccessResponse;
        },
        r => {
          alert(r.error.error); 
          console.log(r.error.error);
        });
    this.loading = false;
  }

  getSingleUserRoleAccess(roleId: number) {
    this.loading = true;
    this.KemarService.get<UserRoleAccessMapping>(null, ServiceUrl.getSingleUserRoleAccessMapping, { roleId: roleId })
      .subscribe(
        SingleuserAccessResponse => {
          // this.singleUserAccessMapping = SingleuserAccessResponse;
          // this.singleUserAccessMapping = SingleuserAccessResponse.userAccessManagerResponse;
          // this.singleUserAccessResponse = SingleuserAccessResponse.userAccessManagerResponse;
          this.singleUserAccessMapping = SingleuserAccessResponse;
          this.singleUserAccessResponse = SingleuserAccessResponse.userAccessManagerResponse;

        },
        r => {
          this.ShowMessage('error', 'Error', r.message);
        });
    this.closeModalPopup();
    this.loading = false;
  }

  assignUserRoleAccessMapping() {
    
    this.loading = true;
    this.KemarService.postPatch<any>(ServiceUrl.assignUserRoleAccessMapping, this.singleUserAccessMapping)
      .subscribe(
        res => {
          this.display = false;
          this.closeModalPopup();
          this.ShowMessage('success', 'success', res.responseMessage);
          this.getAllUserRoleMapping();
        },
        r => {
          this.display = false;
          this.closeModalPopup();
          this.ShowMessage('error', 'error', r.error.errorMessage);
        });
    this.loading = false;
  }

  getAllRoles() {
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.getAllRoles)
      .subscribe(
        response => {

          this.roleMaster = response;
        },
        r => {
          console.log(r.error.error);
        });
    this.loading = false;
  }

  // editModalPopup(roleId: number) {
  // this.loading = true;
  // this.getSingleUserRoleAccess(roleId);
  // this.display = true;

  // }
  ngbModalOptions: NgbModalOptions = {
    backdrop: 'static',
    keyboard: false
  };
  editModalPopup(content: any, roleId: number, accessMapping: any) {
    
    this.loading = true;
    let ngbModalOptions: NgbModalOptions = {
      backdrop: 'static',
      keyboard: false
    };
    if(accessMapping.isRead==2 ){ this.ReadChecked=true;} else {this.ReadChecked=false;}
    if(accessMapping.isCreate==2 ){ this.CreateChecked=true;} else {this.CreateChecked=false;}
    if(accessMapping.isUpdate==2 ){ this.UpdateChecked=true;} else {this.UpdateChecked=false;}
    if(accessMapping.isDelete==2 ){ this.DeleteChecked=true;} else {this.DeleteChecked=false;}
    this.getSingleUserRoleAccess(roleId);

    // this.singleUserAccessMapping.roleId = accessMapping.roleId;
    // this.singleUserAccessMapping.roleName = accessMapping.roleName;
    //this.modalService.open(content, ngbModalOptions);
    this.modalService.open(content, { size: 'xl' });
    this.modalService.open(ngbModalOptions);
  }

  public closeModalPopup() {
    this.modalService.dismissAll();
  }

  SelectAll() {
    this.singleUserAccessMapping.userAccessManagerResponse.forEach((element: any) => {
      element.isActive = true;
      element.canCreate = true;
      element.canUpdate = true;
      element.canDeactivate = true;
    });
    this.ReadChecked=true;
    this.CreateChecked=true;
    this.UpdateChecked=true;
    this.DeleteChecked=true;

  }

  SelectReadOnly(event: any) {
    
    var flag = event.target.checked;
    if (flag){
      this.singleUserAccessMapping.userAccessManagerResponse.forEach((element: any) => {
       element.isActive = true
      });
    }else{
      this.singleUserAccessMapping.userAccessManagerResponse.forEach((element: any) => {
        element.isActive = false;
      });
    }
  }

  SelectCreate(event: any) {
    var flag = event.target.checked;
    if (flag){
      this.singleUserAccessMapping.userAccessManagerResponse.forEach((element: any) => {
        element.canCreate = true;
      });
    }else{
      this.singleUserAccessMapping.userAccessManagerResponse.forEach((element: any) => {
        element.canCreate = false;
      });
    }
  }

  SelectUpdate(event: any) {
    var flag = event.target.checked;
    if (flag){
      this.singleUserAccessMapping.userAccessManagerResponse.forEach((element: any) => {
        element.canUpdate = true;
      });
    }else{
      this.singleUserAccessMapping.userAccessManagerResponse.forEach((element: any) => {
        element.canUpdate = false;
      });
    }
  }

  SelectDelete(event:any) {
    var flag = event.target.checked;
    if (flag){
      this.singleUserAccessMapping.userAccessManagerResponse.forEach((element: any) => {
        element.canDeactivate = true;
      });
    }else{
      this.singleUserAccessMapping.userAccessManagerResponse.forEach((element: any) => {
        element.canDeactivate = false;
      });
    }
  }

  ClearAll() {
    this.singleUserAccessMapping.userAccessManagerResponse.forEach((element: any) => {
      element.isActive = false;
      element.canCreate = false;
      element.canUpdate = false;
      element.canDeactivate = false;
    });
    this.ReadChecked=false;
    this.CreateChecked=false;
    this.UpdateChecked=false;
    this.DeleteChecked=false;
  }

  change(read:any, val:any){
    
    if(read==="read"){ this.ReadChecked=false;}
    if(read==="create"){ this.CreateChecked=false;}
    if(read==="update"){ this.UpdateChecked=false;}
    if(read==="remove"){ this.DeleteChecked=false;}
  }
}
