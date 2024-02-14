import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";

import { FeatherModule } from "angular-feather";
import { allIcons } from "angular-feather/icons";
import { FormsModule, ReactiveFormsModule, FormBuilder } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";

import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { MatExpansionModule } from "@angular/material/expansion";
import { NgxSpinnerModule } from "ngx-spinner";
import { DatePipe } from "@angular/common";
import { authInterceptorProviders } from "./service/auth-interceptor.service";
import { AuthguardServiceService } from "./service/authguard-service.service";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { FullComponent } from "./layouts/full/full.component";
import { DemoFlexyModule } from "./demo-flexy-module";

//barcode
import { NgxBarcodeModule } from "ngx-barcode";

// Modules
import { DashboardModule } from "./dashboard/dashboard.module";
import { ComponentsModule } from "./components/components.module";

// prime ng
import { MessageService } from "primeng/api";
import { TableModule } from "primeng/table";
import { DialogModule } from "primeng/dialog";
import { CardModule } from "primeng/card";
import { AccordionModule } from "primeng/accordion";
import { CheckboxModule } from "primeng/checkbox";
import { MenubarModule } from "primeng/menubar";
import { DropdownModule } from "primeng/dropdown";
import { CalendarModule } from "primeng/calendar";
import { SidebarModule } from "primeng/sidebar";
import { ButtonModule } from "primeng/button";
import { ConfirmDialogModule } from "primeng/confirmdialog";
import { ToastModule } from "primeng/toast";
import { PaginatorModule } from "primeng/paginator";
import { BadgeModule } from "primeng/badge";
import { TooltipModule } from "primeng/tooltip";
import { InputTextModule } from "primeng/inputtext";
import { TabViewModule } from "primeng/tabview";
import { ChartModule } from "primeng/chart";
import { PasswordModule } from "primeng/password";
import { MultiSelectModule } from "primeng/multiselect";

//theme changer

//admin
import { RoleManagementComponent } from "./Admin/role-management/role-management.component";
import { UserRegistrationComponent } from "./Admin/user-registration/user-registration.component";
import { UserRoleMappingComponent } from "./Admin/user-role-mapping/user-role-mapping.component";
import { UserProfileComponent } from "./Admin/user-profile/user-profile.component";
import { LoginComponent } from "./Admin/login/login.component";
import { ForgetPasswordComponent } from "./Admin/forget-password/forget-password.component";
import { CompletedTaskComponent } from "./Master/completed-task/completed-task.component";
import { DepartmentMasterComponent } from "./Master/department-master/department-master.component";
import { DesignationMasterComponent } from "./Master/designation-master/designation-master.component";
import { ProjectMasterComponent } from "./Master/project-master/project-master.component";
import { TasktypeMasterComponent } from "./Master/tasktype-master/tasktype-master.component";
import { NewTaskComponent } from './Transcation/new-task/new-task.component';
import { SupportingInchargeComponent } from './Transcation/supporting-incharge/supporting-incharge.component';
import { TaskTransactionComponent } from './Transcation/task-transaction/task-transaction.component';
import { AllUserReportComponent } from "./Reports/all-user-report/all-user-report.component";
import { ProjectWiseReportComponent } from "./Reports/project-wise-report/project-wise-report.component"; 
import { UserWiseReportComponent } from "./Reports/user-wise-report/user-wise-report.component";
import { VerticleChartComponent } from './Charts/verticle-chart/verticle-chart.component';
import { NotificationsComponent } from './notifications/notifications.component'; 
@NgModule({
  declarations: [
    AppComponent,
    FullComponent,

    RoleManagementComponent,
    UserProfileComponent,
    UserRoleMappingComponent,
    UserRegistrationComponent,
    LoginComponent,
    ForgetPasswordComponent,
    CompletedTaskComponent,
    DepartmentMasterComponent,
    DesignationMasterComponent,
    ProjectMasterComponent,
    TasktypeMasterComponent,
    NewTaskComponent,
    SupportingInchargeComponent,
    TaskTransactionComponent,
    AllUserReportComponent,
    ProjectWiseReportComponent,
    UserWiseReportComponent,
    VerticleChartComponent,
    NotificationsComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    FeatherModule.pick(allIcons),
    DemoFlexyModule,
    DashboardModule,
    HttpClientModule,
    ComponentsModule,
    FormsModule,
    FontAwesomeModule,
    NgbModule,
    MatExpansionModule,
    NgxSpinnerModule,
    ReactiveFormsModule,
    TableModule,
    DialogModule,
    NgxBarcodeModule,
    CardModule,
    AccordionModule,
    CheckboxModule,
    MenubarModule,
    DropdownModule,
    CalendarModule,
    SidebarModule,
    ButtonModule,
    ConfirmDialogModule,
    ToastModule,
    PaginatorModule,
    BadgeModule,
    TooltipModule,
    InputTextModule,
    TabViewModule,
    ChartModule,
    PasswordModule,
    MultiSelectModule,
  ],
  providers: [
    authInterceptorProviders,
    AuthguardServiceService,
    FormBuilder,
    MessageService,
    DatePipe,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
