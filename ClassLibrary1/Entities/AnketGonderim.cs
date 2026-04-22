using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMYS.Core.Entities
{
    [Table("anket_gonderimleri")]
    public class AnketGonderim
    {
        [Key]
        public int GonderimID { get; set; }
        public int? RandevuID { get; set; }
        public int? SurveyID { get; set; }

        [MaxLength(100)]
        public string TokenID { get; set; }
        public bool KvkkOnay { get; set; } = false;
        public DateTime? SonKullanmaTarihi { get; set; }
        public int? HastaID { get; set; }
        public bool KullanildiMi { get; set; } = false;

        [ForeignKey("RandevuID")]
        public Randevu Randevu { get; set; }
        [ForeignKey("SurveyID")]
        public Survey AnketTanim { get; set; }
    }
}

