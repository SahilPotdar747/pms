using Kemar.UrgeTruck.Domain.Common;
using Kemar.UrgeTruck.Domain.DTOs;
using Kemar.UrgeTruck.Domain.ResponseModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;

namespace Kemar.UrgeTruck.ServiceIntegration
{
    public class EmailManager : IEmailManager
    {
        private readonly IConfiguration _configuration;
        public EmailManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ResultModel SendMessage(MailMessageDto mailMessageDto)
        {
            try
            {
                SmtpServerDto smtpServerDto = GetSmtpServerDetails();

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(smtpServerDto.UserId);
                    mail.To.Add(mailMessageDto.MailTo);
                    mail.Subject = mailMessageDto.Subject;
                    mail.Body = mailMessageDto.Body;
                    mail.IsBodyHtml = true;
                   
                    using (SmtpClient smtp = new SmtpClient(smtpServerDto.ServerUrl, smtpServerDto.Port))
                    {
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = new NetworkCredential(smtpServerDto.UserId, smtpServerDto.Password);
                        smtp.EnableSsl = smtpServerDto.EnableSsl;
                        smtp.UseDefaultCredentials = false;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                return ResultModelFactory.CreateFailure(ResultCode.ExceptionThrown, ex.Message);
            }
            return new ResultModel {StatusCode = ResultCode.SuccessfullyCreated,  
                ResponseMessage = "OTP is sent on your registered mail address." };
        }

        private SmtpServerDto GetSmtpServerDetails()
        {
            int parsedPortNumber = 0;
            SmtpServerDto smtpServerDto = new SmtpServerDto();
            smtpServerDto.ServerUrl = _configuration.GetSection("AppSettings").GetSection("SmtpServerUrl").Value; ;
            int.TryParse(_configuration.GetSection("AppSettings").GetSection("SmtpPortNumber").Value, out parsedPortNumber);
            smtpServerDto.Port = parsedPortNumber;
            smtpServerDto.UserId = _configuration.GetSection("AppSettings").GetSection("SmtpUserId").Value; ;
            smtpServerDto.Password = _configuration.GetSection("AppSettings").GetSection("SmtpUserPassword").Value;
            smtpServerDto.EnableSsl = true;
            smtpServerDto.UseDefaultCredentials = true;
            //smtpServerDto.DeliveryMethod = SmtpDeliveryMethod.Network;

            return smtpServerDto;
        }

    }
}

