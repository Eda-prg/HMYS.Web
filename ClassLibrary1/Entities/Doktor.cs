using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HMYS.Core.Entities
{
    [Table("doktorlar")]
    public class Doktor
    {
        [Key]
        public int DoktorID { get; set; }

        [Required, MaxLength(100)]
        public string DoktorAdi { get; set; }

        [MaxLength(20)]
        public string BolumKodu { get; set; }

        [ForeignKey("BolumKodu")]
        public Birim Birim { get; set; }
    }
}
