using HMYS.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.BUsiness
{
    public class LogManager
    {
        private static List<AuditLog> _logs = new List<AuditLog>();

        public void LogAction(string action, string details)
        {
            var log = new AuditLog { Action = action, Details = details };
            _logs.Add(log);

            // Konsola da yazdıralım ki takip edebilelim
            Debug.WriteLine($"[LOG]: {action} - {details}");
        }

        public List<AuditLog> GetLogs() => _logs;

        public void AddLog(string action, string details)
        {
            var log = new AuditLog
            {
                Id = _logs.Count + 1,
                Action = action,
                Details = details
            };
            _logs.Add(log);
        }

        // Tüm logları geriye döner
        public List<AuditLog> GetAllLogs()
        {
            return _logs;
        }
    }
}


