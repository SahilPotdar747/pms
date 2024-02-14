
using Kemar.UrgeTruck.Domain.ResponseModel;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Kemar.TMS.Domain.ResponseModel
{
    public class UserResponse
    {
        public UserResponse()
        {
            projectMaster = new List<ProjectMasterResponse>();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int ReportingUser { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int TotalRecord { get; set; }
        public bool IsActive { get; set; }
        public string empCode { get; set; }

        public RoleMasterResponse RoleMaster { get; set; }
        public DesignationMasterResponse DesignationMaster { get; set; }
        public DepartmentMasterResponse DepartmentMaster { get; set; }
        public ICollection<ProjectMasterResponse> projectMaster { get; set; }
    }


    public class UserResponseNew
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int ReportingUser { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int TotalRecord { get; set; }
        public bool IsActive { get; set; }
        public string empCode { get; set; }

        //public RoleMasterResponse RoleMaster { get; set; }
        //public DesignationMasterResponse DesignationMaster { get; set; }
        //public DepartmentMasterResponse DepartmentMaster { get; set; }
    }
}
