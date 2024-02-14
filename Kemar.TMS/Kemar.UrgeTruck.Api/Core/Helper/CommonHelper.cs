using Kemar.TMS.Repository.Entities;
using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;

namespace Kemar.UrgeTruck.Api.Core.Helper
{
    public static class CommonHelper
    {
        public static void SetUserInformation<T>(ref T sourceEntity, int primayKey, HttpContext httpContext)
        {
            var sessionValue = httpContext.Session.GetString("UserInfo");
            if (sessionValue == null) throw new ApplicationException("Authentication token expired");

            var userInfo = JsonSerializer.Deserialize<AuthenticateResponse>(sessionValue);
            var entity = sourceEntity as CommonEntityModel;
            if (primayKey <= 0)
            {
                entity.CreatedBy = userInfo.FirstName;
                entity.CreatedDate = DateTimeFormatter.GetISDTime(DateTime.Now);
                entity.ModifiedDate = null;
            }
            else
            {
                entity.ModifiedBy = userInfo.FirstName;
                entity.ModifiedDate = DateTimeFormatter.GetISDTime(DateTime.Now);
            }
        }

        //public static void SetUserName<T>(ref T sourceEntity, int primayKey, HttpContext httpContext)
        //{
        //    var sessionValue = httpContext.Session.GetString("UserInfo");
        //    if (sessionValue == null) throw new ApplicationException("Authentication token expired");

        //    var userInfo = JsonSerializer.Deserialize<AuthenticateResponse>(sessionValue);
        //    var entity = sourceEntity as LocationStatusRequest;
        //    if (primayKey <= 0)
        //    {
        //        entity.CreatedBy = userInfo.FirstName;
        //        entity.CreatedOn = DateTimeFormatter.GetISDTime(DateTime.Now);
        //    }
        //    else
        //    {
        //        entity.CreatedBy = userInfo.FirstName;
        //        entity.CreatedOn = DateTimeFormatter.GetISDTime(DateTime.Now);
        //    }
        //}

        public static void SetUserInformationForNewTask<T>(ref T sourceEntity, int primayKey, HttpContext httpContext)
        {
            var sessionValue = httpContext.Session.GetString("UserInfo");
            if (sessionValue == null) throw new ApplicationException("Authentication token expired");

            var userInfo = JsonSerializer.Deserialize<AuthenticateResponse>(sessionValue);
            var entity = sourceEntity as TaskTransaction;
            if (primayKey <= 0)
            {
                entity.CreatedBy = userInfo.FirstName;
                entity.CreatedDate = DateTimeFormatter.GetISDTime(DateTime.Now);
                entity.ModifiedDate = null;
                entity.AssignedDate = DateTimeFormatter.GetISDTime(DateTime.Now);
                entity.AssignedById = httpContext.Session.GetInt32("UserId") == null ? 0 : Convert.ToInt32(httpContext.Session.GetInt32("UserId"));
                entity.AssignedBy = userInfo.FirstName  + " " + userInfo.LastName;
            }
            else
            {
                entity.ModifiedBy = userInfo.FirstName;
                entity.ModifiedDate = DateTimeFormatter.GetISDTime(DateTime.Now);
            }
        }


        public static int GetCurrentUserId(HttpContext httpContext)
        {
            int? userId = httpContext.Session.GetInt32("UserId");
            if (userId == null) throw new ApplicationException("Authentication token expired");

            return Convert.ToInt16(userId);
        }

        public static string ReturnUserName(HttpContext httpContext)
        {
            string? sessionValue = httpContext.Session.GetString("UserInfo");
            if (string.IsNullOrEmpty(sessionValue) || sessionValue == "null") throw new ApplicationException("Authentication token expired");
            var userInfo = JsonSerializer.Deserialize<AuthenticateResponse>(sessionValue);
            return userInfo.FirstName + " " + userInfo.LastName;
        }
    }
}
