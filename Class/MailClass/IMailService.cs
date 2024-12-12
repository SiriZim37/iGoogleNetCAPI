using iGoogleNetCAPI.Class.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iGoogleNetCAPI.Class
{
    public interface IMailService
    {
        Task SendEmailAsync(MailModel mailRequest);
    }
}
