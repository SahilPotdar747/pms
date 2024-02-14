import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

//security purpose --authgaurd--
import { AuthGuard } from "./service/auth-guard.service";

//modules
import { DashboardComponent } from "./dashboard/dashboard.component";
import { FullComponent } from "./layouts/full/full.component";

//admin
import { LoginComponent } from "./Admin/login/login.component";
import { ForgetPasswordComponent } from "./Admin/forget-password/forget-password.component";
import { UserRegistrationComponent } from "./Admin/user-registration/user-registration.component";
import { RoleManagementComponent } from "./Admin/role-management/role-management.component";
import { UserRoleMappingComponent } from "./Admin/user-role-mapping/user-role-mapping.component";
import { UserProfileComponent } from "./Admin/user-profile/user-profile.component";

//master
import { DepartmentMasterComponent } from "./Master/department-master/department-master.component";
import { DesignationMasterComponent } from "./Master/designation-master/designation-master.component";
import { ProjectMasterComponent } from "./Master/project-master/project-master.component";
import { CompletedTaskComponent } from "./Master/completed-task/completed-task.component";
import { TasktypeMasterComponent } from "./Master/tasktype-master/tasktype-master.component";

//page not found
import { PageNotFoundComponent } from "./page-not-found/page-not-found.component";

//Transcation
import { TaskTransactionComponent } from "./Transcation/task-transaction/task-transaction.component";
import { NewTaskComponent } from "./Transcation/new-task/new-task.component";
import { SupportingInchargeComponent } from "./Transcation/supporting-incharge/supporting-incharge.component";
import { ProjectWiseReportComponent } from "./Reports/project-wise-report/project-wise-report.component";
import { UserWiseReportComponent } from "./Reports/user-wise-report/user-wise-report.component";
import { AllUserReportComponent } from "./Reports/all-user-report/all-user-report.component";

//notification
import { NotificationsComponent } from "./notifications/notifications.component";

const routes: Routes = [
  { path: "", redirectTo: "/login", pathMatch: "full" },
  {
    path: "login",
    component: LoginComponent,
  },
  {
    path: "forgot-password",
    component: ForgetPasswordComponent,
  },
  {
    path: "",
    component: FullComponent,

    children: [
      //admin
      {
        path: "Profile",
        component: UserProfileComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "UserRegistration",
        component: UserRegistrationComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "role",
        component: RoleManagementComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "RoleMapping",
        component: UserRoleMappingComponent,
        canActivate: [AuthGuard],
      },

      {
        path: "department",
        component: DepartmentMasterComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "designation",
        component: DesignationMasterComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "project",
        component: ProjectMasterComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "task",
        component: TaskTransactionComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "tasktype",
        component: TasktypeMasterComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "notification",
        component: NotificationsComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "newTask",
        component: NewTaskComponent,
         canActivate: [AuthGuard]
      },
      {
        path: "completedtask",
        component: CompletedTaskComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "projectwisereport",
        component: ProjectWiseReportComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "userwisereport",
        component: UserWiseReportComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "supportingincharge",
        component: SupportingInchargeComponent,
        canActivate: [AuthGuard],
      },
      {
        path: "alluserreport",
        component: AllUserReportComponent,
        canActivate: [AuthGuard],
      },

      //modules ui
      { path: "", redirectTo: "/home", pathMatch: "full" },
      {
        path: "home",
        component: DashboardComponent,
        canActivate: [AuthGuard],
      },
    ],
  },

  { path: "**", component: PageNotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
