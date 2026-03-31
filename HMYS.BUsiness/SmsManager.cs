using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.BUsiness
{
    public class SmsManager
    {
        private readonly ISmsService _smsService;

        public SmsManager(ISmsService smsService)
        {
            _smsService = smsService;
        }
        public async Task<bool> SendSurveySms(string telefon, string mesaj)
        {
            return await _smsService.SendSmsAsync(telefon, mesaj);
        }
    }
}
