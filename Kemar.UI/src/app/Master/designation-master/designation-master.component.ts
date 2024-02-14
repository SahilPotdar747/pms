import { Component, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Table } from 'exceljs';
import { MessageService } from 'primeng/api';
import { ConnectableObservable } from 'rxjs';
import { ExcelServiceService } from 'src/app/service/excel-service.service';
import { KemarServiceService } from 'src/app/service/kemar-service.service';
import { ServiceUrl } from 'src/app/service/service-url.service';


@Component({
  selector: 'app-designation-master',
  templateUrl: './designation-master.component.html',
  styleUrls: ['./designation-master.component.scss']
})
export class DesignationMasterComponent {

  loading = false;
  @ViewChild('dt') dt: Table | undefined;
  title: string = "'Add Designation'";
  designationform!: FormGroup;
  loginUser: string = '';
  submitted!: boolean;
  ShowActive: boolean = false;
  designationMaster: any;
  display: boolean = false;
  Save!: string;
  spinner: boolean = false;
  $index = 0;

  //sorting
  public filter: string = '';
  filterDesignationList: string = '';
  pageSize = 15;
  skipRow: number = 0;
  currentPage = 1;
  totalRecords = 10;
  last: any;

  constructor(private KemarService: KemarServiceService,
    private excelService: ExcelServiceService,
    private fb: FormBuilder,
    private message: MessageService
  ) {
    this.KemarService.isLoggedIn$ = true;
  }

  ngOnInit(): void {
    //this.getAllDesignation();
    this.Reset();
    this.getAllDesignationWithPagination(1);
  }

  Reset() {
    //const nonWhitespaceRegExp: RegExp = new RegExp("\\S");
    this.designationform = this.fb.group({
      designationId: [0, [Validators.required]],
      designationName: [
        '',
        [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(50),
          //Validators.pattern('[a-z A-Z-]*'),
          //Validators.pattern('[^-\\s][A-Za-z0-9()*%!_@./#&+:=-\\s]*'),
          this.noWhitespaceValidator,
          //Validators.pattern(nonWhitespaceRegExp)
        ]
      ],
      isActive: [true, [Validators.required,]],

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
    this.designationform.reset();
    this.Reset();
  }

  ShowMessage(messageType: string, title: string, message: string) {
    this.message.add({ severity: messageType, summary: title, detail: message });
  }

  get f(): { [key: string]: AbstractControl } {
    return this.designationform.controls;
  }

  // applyFilterGlobal($event: any, stringVal: any) {
  //   this.dt!.filterGlobal(($event.target as HTMLInputElement).value, stringVal);
  // }

  pagechange(event: any) {
    
    this.skipRow = event.first;
    this.pageSize = event.rows;
    this.getAllDesignationWithPagination(1);
  }

  getAllDesignation() {   // this method not using for list
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.getAllDesignation)
      .subscribe(
        response => {
          this.designationMaster = response;
        },
        r => {
          this.ShowMessage('error', 'Error', r.error.errorMessage);
        });
    this.loading = false;
  }

  // Search() {
  //   this.skipRow = 0;
  //   this.currentPage = 1;
  //   this.searchDesignWithPagination(1);
  // }

  searchDesignWithPagination(currentPage: any) {  //this function using only for keyup type search without loading...
    
    var query = {
      currentPage: currentPage,
      skipRow: this.skipRow,
      rowSize: this.pageSize,
      searchtext: this.filterDesignationList,
    };
    this.KemarService.get<any>(null, ServiceUrl.getAllDesignationWithPagination, query)
      .subscribe(
        response => {
          this.designationMaster = response;
          if (response?.length > 0) {
            this.totalRecords = response[0].totalRecord;
          }
          else {
            this.totalRecords = 0;
          }
          this.currentPage = currentPage;
          this.loading = false;
        },
        r => {
          this.ShowMessage('error', 'Error', r.error.errorMessage);
          this.loading = false;
        });

  }


  getAllDesignationWithPagination(currentPage: any) {
    
    var query = {
      currentPage: currentPage,
      skipRow: this.skipRow,
      rowSize: this.pageSize,
      searchtext: this.filterDesignationList,
    };
    this.loading = true;
    this.KemarService.get<any>(null, ServiceUrl.getAllDesignationWithPagination, query)
      .subscribe(
        response => {
          this.designationMaster = response;
          if (response?.length > 0) {
            this.totalRecords = response[0].totalRecord;
          }
          else {
            this.totalRecords = 0;
          }
          this.currentPage = currentPage;
          this.loading = false;
        },
        r => {
          this.ShowMessage('error', 'Error', r.error.errorMessage);
          this.loading = false;
        });

  }

  open() {
    
    this.submitted = false;
    this.designationform.reset();
    this.Reset();
    this.ShowActive = true;
    this.title = "Add Designation";
    this.Save = "Add";
    // this.designationform.controls['designationName'].setValue('');
    // this.designationform.controls['isActive'].setValue(true);
    this.display = true;
  }

  editDesignation(designation: any) {
    this.submitted = false;
    this.ShowActive = true;
    this.designationform.reset();
    this.title = "Update Designation";
    this.designationform.patchValue(designation);
    this.Save = "Update";
    this.display = true;
  }

  registerDesignation() {
    
    this.submitted = true;
    if (this.designationform.invalid) {
      return;
    }
    this.loading = true;
    let body = this.designationform.value;
    this.KemarService.postPatch<any>(ServiceUrl.registerDesignation, body)
      .subscribe(
        res => {
          this.ShowMessage('success', 'Success', res.responseMessage);
          this.getAllDesignationWithPagination(1);
        },
        r => {
          this.ShowMessage('error', 'Error', r.error.errorMessage);
        });
    this.display = false;
    this.loading = false;
  }

}
