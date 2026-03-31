using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.Core
{
    public class SoruBankasi
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty; // Soru metni
        public bool IsCritical { get; set; } // [G11] Hot Alert için kritik mi?
        public int Order { get; set; } // Sorunun kaçıncı sırada olduğu
    }
}

