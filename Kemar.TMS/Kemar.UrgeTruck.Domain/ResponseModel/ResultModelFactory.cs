using Kemar.UrgeTruck.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kemar.UrgeTruck.Domain.ResponseModel
{
    public static class ResultModelFactory
    {
        public static ResultModel CreateSucess(string responseMessage = null) => new ResultModel
        {
            StatusCode = ResultCode.SuccessfullyCreated,
            ResponseMessage = responseMessage
        };

        public static ResultModel UpdateSucess(string responseMessage = null) => new ResultModel
        {
            StatusCode = ResultCode.SuccessfullyUpdated,
            ResponseMessage = responseMessage
        };

        public static ResultModel InvalidResult(string responseMessage = null) => new ResultModel
        {
            StatusCode = ResultCode.Invalid,
            ErrorMessage = responseMessage
        };
        public static ResultModel CreateFailure(int statusCode, string errorMessage = null, Exception exception = null) => new ResultModel
        {
            StatusCode = statusCode,
            ErrorMessage = errorMessage,
            Exception = exception
        };

        public static ResultModel InvalidResult(int exceptionThrown, string error_while_addupdate_Task)
        {
            throw new NotImplementedException();
        }
    }
}
