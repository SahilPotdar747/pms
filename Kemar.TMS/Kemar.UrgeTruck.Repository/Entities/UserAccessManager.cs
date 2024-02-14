namespace Kemar.UrgeTruck.Repository.Entities
{
    public class UserAccessManager
    {
        public int UserAccessManagerId { get; set; }
        public int RoleId { get; set; }
        public int UserScreenId { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDeactivate { get; set; }
        public bool IsActive { get; set; }
        public virtual UserScreenMaster UserScreenMaster { get; set; }
        public virtual RoleMaster RoleMaster { get; set; }
    }
}
