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
        [Column("RandevuID")]
        public int RandevuID { get; set; }
        [Column("HastaID", TypeName = "int")]
        public int? HastaID { get; set; }

        [ForeignKey("HastaID")]
        public virtual Hasta Hasta { get; set; }
        [Column("DoktorID")]
        public int? DoktorID { get; set; }
        [Column("RandevuTarihi")]
        public DateTime? RandevuTarihi { get; set; }

        [MaxLength(50)]
        [Column("ProtokolNo")]
        public string ProtokolNo { get; set; }

        [ForeignKey("DoktorID")]
        public Doktor Doktor { get; set; }
    }
}
