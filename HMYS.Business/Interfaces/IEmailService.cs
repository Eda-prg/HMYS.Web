using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.Business.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendSurveyLinkAsync(string toEmail, string hastaAdi, string token);
    }
}
