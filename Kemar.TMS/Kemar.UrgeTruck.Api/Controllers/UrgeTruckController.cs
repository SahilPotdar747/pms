using Kemar.UrgeTruck.Api.Core.Extension;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.RequestModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Kemar.UrgeTruck.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrgeTruckController : ControllerBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UrgeTruckController(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Logger.Debug("Welcome to Kemar Task Management System -API is running");
            return Ok("Welcome to Kemar Task Management System -API is running");
        }

        [HttpGet]
        [Route("me")]
        public IActionResult me()
        {
            Logger.Debug("Welcome to Kemar Task Management System -API is running");
            return Ok("Welcome to Kemar Task Management System -API is running");
        }

        

    }
}
