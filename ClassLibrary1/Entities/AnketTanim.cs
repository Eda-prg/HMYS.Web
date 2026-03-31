using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.Core
{
    public class Survey
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; // Anket başlığı (Örn: Yatan Hasta Memnuniyet)
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsActive { get; set; }
        public string GuidToken { get; set; }= string.Empty; // [G 6.4] Güvenli erişim anahtarı
        public DateTime ExpireDate { get; set; } // [G 6.6] Anketin 48 saat sonra geçersiz olması için
        public bool IsUsed { get; set; }
    }
}
