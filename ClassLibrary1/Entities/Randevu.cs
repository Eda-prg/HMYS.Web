using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HMYS.Core.Entities
{
    [Table("randevular")]
    public class Randevu
    {
        [Key]
        public int RandevuID { get; set; }
        public int? HastaID { get; set; }
        public int? DoktorID { get; set; }
        public DateTime? RandevuTarihi { get; set; }

        [MaxLength(50)]
        public string ProtokolNo { get; set; }

        [ForeignKey("HastaID")]
        public Hasta Hasta { get; set; }
        [ForeignKey("DoktorID")]
        public Doktor Doktor { get; set; }
    }
}
