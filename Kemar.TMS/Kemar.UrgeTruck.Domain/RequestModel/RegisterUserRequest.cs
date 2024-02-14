using Kemar.UrgeTruck.Domain.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kemar.UrgeTruck.Domain.RequestModel
{

    public class RegisterUserRequest : CommonEntityModel
    {
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string UserName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(3)]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]

        public string Email { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(250)]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{5,}$", ErrorMessage = "Passwords must be at least 5 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The passwords do not match.")]
        public string ConfirmPassword { get; set; }
  
        [Required(ErrorMessage = "Please enter role")]
       // [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        public int RoleId { get; set; }

        [Range(typeof(bool), "true", "true")]
        public bool AcceptTerms { get; set; }

        public int ReportingUser { get; set; }
        public int DepartmentId { get; set; }

        public int DesignationId { get; set; }
        //public string VerificationToken { get; set; }
        //public string PasswordHash { get; set; }
        //public bool IsActive { get; set; }
        public string MobileNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        private bool DateTime(string v)
        {
            throw new NotImplementedException();
        }

        public string empCode { get; set; }
    }


    public class updateUserRequest : CommonEntityModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [MinLength(3)]
        [MaxLength(50)]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        public int RoleId { get; set; }
        public bool AcceptTerms { get; set; }

        public int ReportingUser { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public string MobileNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string empCode { get; set; }
    }
}
