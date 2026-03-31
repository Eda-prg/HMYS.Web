using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMYS.Core.Entities
{
    [Table("soru_tipleri")]
    public class SoruTipi
    {
        [Key]
        public int SoruTipiID { get; set; }

        [Required, MaxLength(50)]
        public string TipAdi { get; set; }
    }
}
