using System;

namespace Kemar.UrgeTruck.Domain.ResponseModel
{
    public class ResultModel
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public string ResponseMessage { get; set; }
    }
}
