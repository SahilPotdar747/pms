using Kemar.UrgeTruck.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kemar.UrgeTruck.Api.Core.Auth
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<string> _roles;
        private readonly string _screenCode;
        private readonly string _access;

        public AuthorizeAttribute(string screenCode = null, string access = null, params string[] roles)
        {
            _roles = roles ?? new string[] { };
            _screenCode = screenCode;
            _access = access;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAccess = true;
            var account = (UserTokenDto)context.HttpContext.Items["Account"];

            if (account == null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }
            if (_screenCode != null)
                isAccess = account.UserAccess.Any(x => x.ScreenCode == _screenCode && CheckAccess(x,_access));

            if (account == null || !isAccess)
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }

        private bool CheckAccess(UserAccessDto dto, string access = null)
        {
            if (access == "C")
                return dto.CanCreate;
            else if (access == "U")
                return dto.CanUpdate;
            else if (access == "D")
                return dto.CanDeactivate;
            else if (dto.CanRead == true)
                return dto.CanRead;
            else
                return false;
        }
    }
}
