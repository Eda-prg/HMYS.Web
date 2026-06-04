using HMYS.Core;
using HMYS.Business.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace HMYS.Business
{
    public class LogManager : ILogManager
    {
        private static List<AuditLog> _logs = new List<AuditLog>();

        public void LogAction(string action, string details)
        {
            AddLog(action, details); // DRY — tekrar yazmak yerine AddLog'u çağır
            Debug.WriteLine($"[LOG]: {action} - {details}");
        }

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

        public List<AuditLog> GetAllLogs() => _logs;
        public List<AuditLog> GetLogs() => _logs;
    }
}