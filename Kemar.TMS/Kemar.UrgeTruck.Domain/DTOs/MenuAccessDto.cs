using System.Collections.Generic;

namespace Kemar.UrgeTruck.Domain.DTOs
{
    public class MenuAccessDto
    {
        public string DisplayName { get; set; }
        public int ParentId { get; set; }
        public string RoutingURL { get; set; }
        public string MenuIcon { get; set; }

        public MenuAccessDto ParentCategory { get; set; }
        public List<MenuAccessDto> Children { get; set; }
    }
}
