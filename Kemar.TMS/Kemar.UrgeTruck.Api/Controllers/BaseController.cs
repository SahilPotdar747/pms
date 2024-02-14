using Kemar.UrgeTruck.Repository.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kemar.UrgeTruck.Api.Controllers
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        string str = "";
        // returns the current authenticated account (null if not logged in)
        public UserManager Account => (UserManager)HttpContext.Items["Account"];
    }
}
