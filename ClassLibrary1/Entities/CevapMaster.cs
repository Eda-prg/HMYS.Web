using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMYS.Core.Entities
{
    [Table("cevap_master")]
    public class CevapMaster
    {
        [Key]
        public int ResponseID { get; set; }
        public int? GonderimID { get; set; }
        public DateTime? TamamlanmaTarihi { get; set; } = DateTime.Now;
        public int? ToplamSkor { get; set; }

        [ForeignKey("GonderimID")]
        public AnketGonderim AnketGonderim { get; set; }
    }
}
