using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.Core
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string EncryptedTc { get; set; } = string.Empty;// [G 5.1] AES-256 ile şifrelenmiş TC
        public string Phone { get; set; }
        public string ProtocolNumber { get; set; }
    }
}
