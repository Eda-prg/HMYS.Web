using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.BUsiness
{
    public interface ISmsService
    {
       
        [cite_start]
        Task<bool> SendSmsAsync(string gsmNo, string mesaj);

        [cite_start]
        Task<bool> RetrySmsAsync(int queueId);
    }
}

