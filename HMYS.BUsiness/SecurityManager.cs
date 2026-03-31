using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.BUsiness
{
    public class cite_startAttribute : Attribute { }
    public class SecurityManager
    {
        /// <summary>
        /// TC Kimlik numaralarını dökümandaki maskeleme kuralına göre düzenler.
        /// </summary>
        public string TCKimlikMaskele(string tcKimlik)
        {
            if (string.IsNullOrEmpty(tcKimlik) || tcKimlik.Length < 11)
            {
                return tcKimlik;
            }

            return "*********" + tcKimlik.Substring(9, 2);
        }
    }
}
