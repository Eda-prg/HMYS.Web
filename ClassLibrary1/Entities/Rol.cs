using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMYS.Core.Entities
{
    [Table("roller")]
    public class Rol
    {
        [Key]
        public int RolID { get; set; }

        [Required, MaxLength(50)]
        public string RolAdi { get; set; }

        public ICollection<Kullanici> Kullanicilar { get; set; }
    }
}

