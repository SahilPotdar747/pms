export const ServiceUrl = {
  // User Configuration
  GetUserNavDesignConfig: 'ApplicationConfiguration/GetUserNavDesignConfig',

  // User registration
  registerUser: 'UserManagement/RegisterUser',
  authenticate: 'UserManagement/authenticate',
  forgotPassword: 'UserManagement/forgot-password',
  resetPassword: 'UserManagement/reset-password',
  changePassword: 'UserManagement/change-password',
  resetPasswordByAdmin: 'UserManagement/reset-password-by-admin',

  // USER registration
  registerUserManager: 'UserManagement/RegisterUser',
  getUsersData: 'UserManagement/GetAllUsers',
  GetActiveUsers: 'UserManagement/GetActiveUsers',
  getUsersWithPagination: 'UserManagement/getUsersWithPagination',
  updateUserDeatils: 'UserManagement/UpdateUser',
  ressetPassword: 'UserManagement/reset-password',
  refreshToken: 'UserManagement/refreshtoken',
  getAllUserbyDepartment: 'UserAccessManager/getAllUserbyDepartment',
  updateUserImageProfile: 'UserManagement/UpdateUsersProfileImageAsync',
  getAllUserNew: 'UserManagement/GetAllUsersAsyncNew',

  // role registration
  registerRole: 'RoleManager/registerrole',
  getAllRoles: 'RoleManager/GetAllRoles',
  getRole: 'RoleManager/GetRole',
  getAllActiveRoles: 'RoleManager/GetAllActiveRoles',
  getAllRolesWithPagination: 'RoleManager/getRoleWithPagination',

  //Department Registration
  getAllDepartment: 'Department/getAllDept',
  getAllDeptWithPagination: 'Department/getDeptWithPagination',
  registerDepartment: 'Department/registerDepartment',
  getActiveDepartment: 'Department/getActiveDept',
  getDeptWithSort:'Department/getDeptWithSort',

  //Designation Registration
  getAllDesignation: 'Designation/getAllDesignation',
  getAllDesignationWithPagination: 'Designation/getDesignationWithPagination',
  getDesignWithSort: 'Designation/getDesignWithSort',
  registerDesignation: 'Designation/registerDesignation',
  getActiveDesignation: 'Designation/getActiveDesignation',

  //Project Registration
  getAllProject: 'Project/getAllProject',
  getAllProjectWithPagination: 'Project/getProjectWithPagination',
  registerProject: 'Project/registerProject',
  getActiveProject: 'Project/getActiveProject',
  getManagerDDL: 'Project/getManagerDDL',

  //Task Registration
  getAllTask: 'TaskType/getAllTask',
  getAllTaskWithPagination: 'TaskType/getTaskWithPagination',
  registerTaskType: 'TaskType/registerTaskType',
  getActiveTask: 'TaskType/getActiveTask',
  getAllTaskTypeDepartmentWise: 'TaskType/getAllTaskTypeDepartmentWiseAsync',

  //Notification
  getMyNotification: 'Notification/GetMyNotification',
  closeNotificationByUser: 'Notification/CloseNotificationByUser',
  getCurrentNotification: 'Notification/GetCurrentNotifications',
  getMyActiveNotificationCount: 'Notification/GetMyNotificationCount',
  GetDesktopNotifications: 'Notification/GetDesktopNotifications',
  CloseNotificationByUserForDesktop:
    'Notification/CloseNotificationByUserForDesktop',
  CloseAllNotificationByUser: 'Notification/CloseAllNotificationByUser',

  // User Role Access manager
  getSingleUserRoleAccessMapping: 'UserAccessManager/getUserAccessbyRole',
  getAllUserRoleAccessMapping: 'UserAccessManager/getAllUserRoleAccess',
  assignUserRoleAccessMapping: 'UserAccessManager/assignUserAccessRole',

  // Main dashboard URL
  getMainDashboard: 'DashBoard/getMainDashboardData',
  getEOFDetailofInventory: 'Dashboard/GetEOFDetailofInventory',

  // TaskTransaction
  GetAllUsersOfParentUserAsync: 'TaskTransaction/GetAllUsersOfParentUserAsync',
  RegisterTask: 'TaskTransaction/RegisterTaskAsync',
  GetAllMyTask: 'TaskTransaction/GetAllMyTaskAsync',
  GetAllMyTeamTask: 'TaskTransaction/GetAllMyTeamTaskAsync',
  CheckIHaveTeam: 'TaskTransaction/CheckIHaveTeam',
  getTaskTransactionWithPagination:
    'TaskTransaction/getTaskTransactionWithPagination',
  getTeamTaskTransactionWithPagination:
    'TaskTransaction/getTeamTaskTransactionWithPagination',
  getAllRaisedByMeTask: 'TaskTransaction/GetAllRaisedByMeTask',
  getAllRaisedTaskWithPagination:
    'TaskTransaction/getAllRaisedTaskWithPagination',
    ReOpenTask: 'TaskTransaction/ReOpenTask',

  // Delegate Task
  getAllDeleatedTask: 'DelegateHistory/GetAllMyDelegatedTask',
  RegisterDeleateTask: 'DelegateHistory/RegisterDelegateAsync',
  UpdateDeleatedTaskAction: 'DelegateHistory/DelegateActionAsync',
  checkIHaveDelegatedTask: 'DelegateHistory/CheckIHaveDelegateTask',
  getMyRaisedDelegatedTask: 'DelegateHistory/GetMyRaisedDelegateTask',
  reopenCompletedTask: 'TaskTransaction/ReopenCompletedTask',

  // New Task
  getAllUnAssignedTask: 'TaskTransaction/GetAllUnAssignTask',

  // Task History
  getTaskHistory: 'TaskHistory/GetTaskHistory',

  // Dashboard
  getDashboardData: 'Dashboard/GetDashboardData',
  getDashboardPiaData: 'Dashboard/GetDashboardPiaData',

  // Completed Task
  GetUsersOfParentUserAsync: 'TaskTransaction/GetAllUsersOfParentUserAsync',
  checkIHaveTeam: 'TaskTransaction/CheckIHaveTeam',
  getCompletedTaskWithPagination:
    'TaskTransaction/getCompletedTaskWithPagination',
  getCompletedTeamTaskWithPagination:
    'TaskTransaction/getCompletedTeamTaskWithPagination',
    getCompletedRaisedTaskWithPagination:'TaskTransaction/getCompletedRaisedTaskWithPagination',
  getCompletedTaskToDownload: 'TaskTransaction/getCompletedTaskToDownload',
  getCompletedTeamTaskToDownload:
    'TaskTransaction/getCompletedTeamTaskToDownload',

  //Task Report
  GetProjectWiseTaskCountReport: 'Report/GetProjectWiseTaskCountReport',
  GetUserWiseTaskDataCountReport: 'Report/GetUserWiseTaskDataCountReport',
  GetProjectWiseTask: 'Report/GetProjectWiseTask',
  GetUserWiseTask: 'Report/GetUserWiseTask',
  GetProjectWiseTaskCountReportToDownload:
    'Report/GetProjectWiseTaskCountReportToDownload',
  GetUserWiseTaskCountReportToDownload:
    'Report/GetUserWiseTaskDataCountReportToDownload',
  GetUserWiseReportToDownload: 'Report/GetUserWiseTaskToDownload',
  GetProjectWiseTaskToDownload: 'Report/GetProjectWiseTaskToDownload',
  GetAllUserWiseTaskDataCountReportToDownload:
    'Report/GetAllUserWiseTaskDataCountReportToDownload',
  GetAllUserWiseReportToDownload: 'Report/GetAllUserWiseReportToDownload',

  //Supporting Incharge
  CoordinatingTeamTask: 'TaskTransaction/CoordinatingTeamTaskAsyn',
  CoordinatingTeamTaskToDownload:
    'TaskTransaction/CoordinatingTeamTaskToDownloadAsyn',

  //All User Report
  GetAllUserWiseTaskDataCountReport: 'Report/GetAllUserWiseTaskDataCountReport',
  GetAllUserWiseTask: 'Report/GetAllUserWiseTask',
};
