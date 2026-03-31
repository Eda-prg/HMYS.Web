using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HMYS.Core.Entities;

namespace HMYS.Core.Entities
{
    [Table("anket_sorulari")]
    public class AnketSoru
    {
        [Key]
        public int AnketSoruID { get; set; }
        public int? SurveyID { get; set; }
        public int? QuestionID { get; set; }
        public int? SiraNo { get; set; }

        [ForeignKey("SurveyID")]
        public Survey  AnketTanim { get; set; }
        [ForeignKey("QuestionID")]
        public SoruBankasi SoruBankasi { get; set; }
    }
}
