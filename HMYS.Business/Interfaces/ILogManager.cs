using HMYS.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.Business.Interfaces
{
    public interface ILogManager
    {
        void LogAction(string action, string details);
        void AddLog(string action, string details);
        List<AuditLog> GetAllLogs();
        List<AuditLog> GetLogs();
    }
}
