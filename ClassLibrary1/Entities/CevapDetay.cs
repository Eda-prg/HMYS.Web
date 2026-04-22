using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HMYS.Core.Entities // Eğer namespace farklıysa kendi projene göre düzelt
{
    [Table("cevap_detay")]
    public class CevapDetay
    {
        [Key]
        public int DetailID { get; set; }

        public int? ResponseID { get; set; }

        public int? QuestionID { get; set; }

        public int? Puan { get; set; }

        public string MetinCevap { get; set; }

        // --- İLİŞKİLER (FOREIGN KEYS) ---
        // Bunlar veritabanında sütun oluşturmaz, sadece tabloları bağlar.
        // SoruBankasi ve CevapMaster sınıfların projende varsa bunlar kalabilir:

        [ForeignKey("ResponseID")]
        public CevapMaster CevapMaster { get; set; }

        [ForeignKey("QuestionID")]
        public SoruBankasi SoruBankasi { get; set; }
    }
}