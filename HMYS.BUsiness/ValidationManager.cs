using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HMYS.BUsiness
{
    public class ValidationManager
    {
        public bool IsValidTc(string tcNo)
        {
            if (string.IsNullOrEmpty(tcNo)) return false;
            return Regex.IsMatch(tcNo, @"^[0-9]{11}$");
        }

        // Telefon format kontrolü (Türkiye formatı)
        public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;
            // 05xx xxx xx xx formatı için basit bir kontrol
            return Regex.IsMatch(phone, @"^(05)[0-9]{9}$");
        }
        public string ValidateSurveyResponses(List<Response> cevaplar)
        {
            // 1. Kural: Liste tamamen boş gönderilemez
            if (cevaplar == null || !cevaplar.Any())
                return "Hata: Anket boş gönderilemez, en az bir soruya cevap vermelisiniz.";

            foreach (var cevap in cevaplar)
            {
                // 2. Kural: Puanlama sadece 1 ile 5 arasında olmalıdır
                if (cevap.Score < 1 || cevap.Score > 5)
                    return $"Hata: Geçersiz puan ({cevap.Score}). Puanlama sadece 1 ile 5 arasında olmalıdır.";

                // 3. Kural: Soru metni boş bırakılamaz veya sadece boşluklardan oluşamaz
                if (string.IsNullOrWhiteSpace(cevap.Question))
                    return "Hata: Soru metni eksik veya boş bırakılamaz.";
            }

            // Hiçbir kural ihlali yoksa sistem güvenli demektir
            return string.Empty;
        }
    }
}
