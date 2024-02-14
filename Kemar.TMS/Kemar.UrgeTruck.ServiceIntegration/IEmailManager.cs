using Kemar.UrgeTruck.Domain.DTOs;
using Kemar.UrgeTruck.Domain.ResponseModel;

namespace Kemar.UrgeTruck.ServiceIntegration
{
    public interface IEmailManager
    {
        ResultModel SendMessage(MailMessageDto mailMessageDto);
    }
}