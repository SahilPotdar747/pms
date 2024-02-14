using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Domain.Common
{
    public static class UrgeTruckMessages
    {

        // Regex Patterns 
        // Accepts character AtoZ in both case Upper and lower, and also 0-9 and whiteSpace
        // Pattern1 length is 10 ,Pattern2 length is 30 ,Pattern3 length is 300 
        public const string regx_pattern1 = @"^\b[A-Za-z0-9\s]{0,10}\b+$"; 
        public const string regx_pattern2 = @"^\b[A-Za-z0-9\s]{0,30}\b+$";
        public const string regx_pattern3 = @"^[A-Za-z0-9\s\.\,\(\)]{0,300}$";

        // Global 
        public const string added = " Successfully.";
        public const string updated = " Successfully.";
        public const string added_successfully = " added successfully.";
        public const string reopen_successfully = " re-open successfully.";
        public const string no_record_found = "no record found.";
        public const string initiated_successfully = "initiated successfully.";
        public const string updated_successfully = " updated successfully.";
        public const string successfully_updated = "successfully updated";
        public const string ax4user = "ax4user";

        public const string NOT_A_VALID_MODEL = "Not a Valid Model";

        

        // RoleRegistrationRepository
        public const string Error_while_addupdate_role = "Error while add/update role";
        public const string role_Exist = "Role Already Exist";
        public const string role_Assign_to_User = "Role is already Assign to Users";
        public const string dept_Assign_to_User = "Department is already Assign to Users";
        public const string design_Assign_to_User = "Designation is already Assign to Users";

        // DepartmentRegistrationRepository
        public const string Error_while_addupdate_dept = "Error while add/update department";
        public const string dept_Exist = "Department Already Exist";

        // DesignationRegistrationRepository
        public const string Error_while_addupdate_designation = "Error while add/update designation";
        public const string designation_Exist = "Designation Already Exist";

        // ProjectRegistrationRepository
        public const string Error_while_addupdate_project = "Error while add/update project";
        public const string project_Exist = "Project Already Exist";

        // TaskTypeRegistrationRepository
        public const string Error_while_addupdate_taskType = "Error while add/update Action";
        public const string taskType_Exist = "Action Already Exist";

        // ServiceIntegrationTrackingRepository
        public const string Service_tracking = "Service tracking ";
        public const string Error_while_addupdate_Thirdparty_service_tracking = "Error while add/update Thirdparty service tracking.";

        // TransporterRegistrationRepository
        public const string Cant_register_duplicate_Transporter_name = "Can't register duplicate Transporter name.";
        public const string Transporter_record = "Transporter record";
        public const string Error_while_addupdate_Transporter = "Error while add/update Transporter";

        // UserAccessManagerRepository
        public const string User_access_assinged_successfully = "User access assinged successfully";
        public const string Error_while_addupdate_user_access = "Error while add/update user accessw";

        // UserManagerRepository
        public const string User_name_or_password_is_incorrect = "User name or password is incorrect";
        public const string Duplicate_records_exists = "Duplicate records exists";
        public const string No_super_admin_role_exists_to_create_first_user = "No super admin role exists to create first user.";
        public const string User_registered_succesfully = "User registered succesfully.";
        public const string User_Name_is_invalid = "User Name is invalid";
        public const string ResetPasswordAsync_error = "ResetPasswordAsync - error:-";
        public const string Invalid_user_name_or_password = "Invalid user name or password.";
        public const string Your_password_has_been_changed_successfully = "Your password has been changed successfully";
        public const string User_information_udpated_successfully = "User information udpated successfully";
        public const string UpdateUserDeatilAsync_error = "UpdateUserDeatilAsync- error:-";
        public const string Error_while_addupdate_User_details = "Error while add/update User details";
        public const string RegisterUserAsync_error = "RegisterUserAsync -error:-";
        public const string UserPhoneExist = "User Phone Number Already Exist";
        public const string UserNameExist = "User Name Already Exist";
        public const string Error_While_UploadImage = "Error while Update Profile Image";
        public const string UpdatedProfileImage = "Profile Image Updated Succesfully";

        // UserScreenRepository
        public const string User_screen = "User screen ";
        public const string Error_while_addupdate_user_screen = "Error while add/update user screen";

        

        //CommonMasterData
        public const string Cant_register_duplicate_ReasonType = "Can't register duplicate ReasonType.";
        public const string CommonMasterData_Successfully = "Common Master Data Added Successfully.";
        public const string Error_while_addupdate_CommonMasterData = "Error while add/update CommonMasterData";
        public const string CommonMasterData_updated_successfully = "Common Master Data Updated Successfully.";
        public const string CommonMasterData_value_alreadyexist = "Value already Exist";


        // NotificationRepository
        public const string Error_while_addupdate_Notification = "Error while add/update Notification";

        // add/update Task";
        public const string Task_Exist = "Task Already Exist";
        public const string Task_Title_Exist = "Task Title Already";
        public const string Error_while_addupdate_Task = "Error while add/update Task";
        public const string Task_Added_To_Your_Task = "Task Added To Your Task";
        public const string Task_Rejected = "Task Rejected";
        public const string Error_while_reopen_Task = "Error while Reopen Task";
        public const string Task_reopen_successfully = "Reopen Successfully.";
    }
}
