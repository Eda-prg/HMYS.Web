using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.Core
{
   public class AuditLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Action { get; set; }
        public string Details { get; set; } // Örn: "TC Sorgulandı: *********74"
        public DateTime LogDate { get; set; } = DateTime.Now;
    }
}

