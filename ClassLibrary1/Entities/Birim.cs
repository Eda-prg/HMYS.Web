using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMYS.Core.Entities
{
    [Table("birimler")]
    public class Birim
    {
        [Key]
        [MaxLength(20)]
        public string BolumKodu { get; set; }

        [Required, MaxLength(100)]
        public string BirimAdi { get; set; }

        public ICollection<Kullanici> Kullanicilar { get; set; }
        public ICollection<Doktor> Doktorlar { get; set; }
    }
}
