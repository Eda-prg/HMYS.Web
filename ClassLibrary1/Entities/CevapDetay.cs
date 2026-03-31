using HMYS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMYS.Core.Entities
{
    [Table("cevap_detay")]
    public class CevapDetay
    {
        [Key]
        public int Id { get; set; } // Senin eski Primary Key'in

        public int? SurveyId { get; set; } // Senin eski anket ID alanın
        public int? Score { get; set; } // Senin puan alanın
        public string Comment { get; set; } // Senin yorum alanın

        public int? ResponseID { get; set; } // İlişki için ana cevap ID
        public int? QuestionID { get; set; } // Teke düşürülmüş QuestionID!

        [ForeignKey("ResponseID")]
        public CevapMaster CevapMaster { get; set; }

        [ForeignKey("QuestionID")]
        public SoruBankasi SoruBankasi { get; set; }
    }
}
