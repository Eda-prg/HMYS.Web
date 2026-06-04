using System;
using System.Collections.Generic;
using System.Linq;
using HMYS.Business.Interfaces;

namespace HMYS.Business
{
    public class AnketManager
    {
        private readonly IAnalyticsManager _analytics;
        private readonly ILogManager _logger;

        // Dependency Injection
        public AnketManager(IAnalyticsManager analytics, ILogManager logger)
        {
            _analytics = analytics;
            _logger = logger;
        }

        private const int KritikPuanSiniri = 2;

        public bool HotAlertKontrol(int puan, bool kritikSoruMu)
        {
            return kritikSoruMu && puan <= KritikPuanSiniri;
        }

        public AnketSonucDto AnketTamamla(List<Response> cevaplar)
        {
            if (cevaplar == null || !cevaplar.Any())
                throw new ArgumentException("Cevap listesi boş olamaz.");

            var puanlar = cevaplar.Select(c => c.Score).ToList();
            double ortalama = _analytics.CalculateAverageScore(puanlar);

            bool alarmVarMi = cevaplar.Any(c => HotAlertKontrol(c.Score, c.IsKritikSoru));

            _logger.LogAction("Anket Tamamlandı", $"Ortalama Puan: {ortalama}, Alarm: {alarmVarMi}");

            return new AnketSonucDto
            {
                OrtalamaPuan = ortalama,
                HotAlertDurumu = alarmVarMi,
                Mesaj = alarmVarMi ? "Düşük puan nedeniyle birim sorumlusu bilgilendirildi." : "Katılımınız için teşekkürler.",
                TamamlanmaTarihi = DateTime.Now
            };
        }
    }

    public class Response
    {
        public int Score { get; set; }
        public string Question { get; set; }
        public bool IsKritikSoru { get; set; }
    }

    public class AnketSonucDto
    {
        public double OrtalamaPuan { get; set; }
        public bool HotAlertDurumu { get; set; }
        public string Mesaj { get; set; }
        public DateTime TamamlanmaTarihi { get; set; }
    }
}