using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HMYS.Core.Entities
{
    [Table("hastalar")]
    public class Hasta
    {
        [Key]
        [Column("HastaID", TypeName = "int")]
        public int HastaID { get; set; }

        [Required, MaxLength(255)]
        public string TCKimlikEncrypted { get; set; }

        [MaxLength(20)]
        public string HastaGsmNo { get; set; }

        public ICollection<Randevu> Randevular { get; set; }
    }
}
