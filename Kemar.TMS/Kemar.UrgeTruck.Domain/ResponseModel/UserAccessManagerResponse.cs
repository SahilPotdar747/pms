namespace Kemar.UrgeTruck.Domain.ResponseModel
{
    public class UserAccessManagerResponse
    {
        public int UserAccessManagerId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int UserScreenId { get; set; }
        public string ScreenName { get; set; }
        public string ParentName { get; set; }
        public int ParentId { get; set; }
        public string ScreenCode { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDeactivate { get; set; }
        public bool IsActive { get; set; }
        public string MenuIcon { get; set; }
    }
}
