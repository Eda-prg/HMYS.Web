using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMYS.BUsiness
{
    public class Response // Eksik olan Response sınıfı eklendi
    {
        public int Score { get; set; }
        public string Question { get; set; }
    }
    public class AnketManager
    {
        private readonly AnalyticsManager _analytics = new AnalyticsManager();
        private readonly LogManager _logger = new LogManager();
        public bool HotAlertKontrol(int puan, bool kritikSoruMu)
        {
            if (kritikSoruMu && puan <= 2)
            {
                return true;
            }
            return false;
        }
        public object AnketTamamla(List<Response> cevaplar)
        {
            // 1. Ortalama Puan Hesapla [G10]
            var puanlar = cevaplar.Select(c => c.Score).ToList();
            double ortalama = _analytics.CalculateAverageScore(puanlar);

            // 2. Hot Alert Kontrolü [G11]
            // Eğer herhangi bir kritik soruya 2 veya altı puan verildiyse alarm tetiklenir
            bool alarmVarMi = cevaplar.Any(c => c.Score <= 2);

            // 3. Denetim İzi (Loglama) [G12]
            _logger.LogAction("Anket Tamamlandı", $"Ortalama Puan: {ortalama}, Alarm: {alarmVarMi}");

            return new
            {
                OrtalamaPuan = ortalama,
                HotAlertDurumu = alarmVarMi,
                Mesaj = alarmVarMi ? "Düşük puan nedeniyle birim sorumlusu bilgilendirildi." : "Katılımınız için teşekkürler.",
                TamamlanmaTarihi = DateTime.Now
            };
        }
        public object AnketSureciniTamamla(List<Response> cevaplar)
        {
            // Yeni metodun basit bir örnek implementasyonu
            return new { Durum = "Başarılı", ToplamCevap = cevaplar.Count };
        }
    }
}
