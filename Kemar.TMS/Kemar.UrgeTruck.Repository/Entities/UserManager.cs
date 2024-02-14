using Kemar.TMS.Domain.ResponseModel;
using Kemar.TMS.Repository.Entities;
using Kemar.TMS.Repository.Repositories;
using System;
using System.Collections.Generic;

namespace Kemar.UrgeTruck.Repository.Entities
{
    public class UserManager
    {
        public UserManager()
        {
            ProjectMaster = new List<ProjectMaster>();
            TaskTransaction = new List<TaskTransaction>();
            Notification = new List<Notification>();
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string PasswordHash { get; set; }
        public bool AcceptTerms { get; set; }
        public int RoleId { get; set; }
        public int DesignationId { get; set; }
        public int DepartmentId { get; set; }
        public string VerificationToken { get; set; }
        public DateTime? Verified { get; set; }
        public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
        public string ResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
        public DateTime? PasswordReset { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public int? ReportingUser { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string empCode { get; set; }
        public string ProfileImagePath { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }

        public bool OwnsToken(string token)
        {
            return this.RefreshTokens?.Find(x => x.Token == token) != null;
        }
        public virtual RoleMaster RoleMaster { get; set; }
        public virtual DesignationMaster DesignationMaster { get; set; }
        public virtual DepartmentMaster DepartmentMaster { get; set; }
        public virtual ICollection<ProjectMaster> ProjectMaster { get; set; }
        public virtual ICollection<TaskTransaction> TaskTransaction { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }

    }
}
