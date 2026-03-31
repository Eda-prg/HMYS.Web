using HMYS.BUsiness;
using HMYS.Core;
using Microsoft.AspNetCore.Mvc;

namespace HMYS.Web.Controllers

{

    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly AnketManager _anketManager = new AnketManager();
        private readonly SecurityManager _securityManager = new SecurityManager();
        private readonly LinkManager _linkManager = new LinkManager();
        private readonly ValidationManager _validation = new ValidationManager();
        private readonly AnalyticsManager _analytics = new AnalyticsManager();
        private readonly LogManager _logger = new LogManager();
        private static List<Survey> _aktifAnketler = new List<Survey>();
        private readonly LogManager _logManager = new LogManager();


        [HttpGet("test-hmys")]
        public IActionResult TestHmys(string tcNo, int puan, bool kritikSoru)
        {
            // [G 5.3]: TC Kimlik numarası maskeleme kuralını uygula
            var maskeliTc = _securityManager.TCKimlikMaskele(tcNo);

            // [G11]: Hot Alert (Kötü Puan Alarmı) kontrolünü yap (Döküman Maddesi: 108, 109)
            var alarmVarMi = _anketManager.HotAlertKontrol(puan, kritikSoru);

            // Sonucu döküman standartlarında JSON olarak dön
            return Ok(new
            {
                MaskelenmisKimlik = maskeliTc,
                VerilenPuan = puan,
                KritikSoruDurumu = kritikSoru,
                HotAlertTetiklendiMi = alarmVarMi,
                Mesaj = alarmVarMi ? "Kritik Düşük Puan Alarmı!" : "Normal"
            });
        }



        [HttpGet("hazirla-ve-gonder")]
        public IActionResult AnketHazirla(string telefonNo)
        {
            // [G 6.4]: Güvenli GUID üretimi
            var token = _linkManager.GenerateSecureToken();
            var sonTarih = _linkManager.GetExpirationDate();


            // [G 13.3]: Dinamik link yapısı
            var link = _linkManager.CreateSurveyUrl("https://localhost:7114", token);

            return Ok(new
            {
                GidenNumara = telefonNo,
                UretilenToken = token,
                AnketLink = link,
                GecerlilikBitis = sonTarih.ToString("dd.MM.yyyy HH:mm"),
                Durum = "SMS Gönderime Hazır"
            });
        }


        [HttpGet("tam-test")]
        public IActionResult TamDenetim(string tcNo, string telefon, int puan)
        {
            // 1. Validasyon Kontrolü
            if (!_validation.IsValidTc(tcNo)) return BadRequest("Geçersiz TC Kimlik formatı!");
            if (!_validation.IsValidPhone(telefon)) return BadRequest("Geçersiz Telefon formatı!");

            // 2. İş Kuralları
            var maskeliTc = _securityManager.TCKimlikMaskele(tcNo);
            var alarm = _anketManager.HotAlertKontrol(puan, true); // Kritik soru varsayalım

            // 3. Hesaplama (Örnek liste ile)
            var ortalama = _analytics.CalculateAverageScore(new List<int> { 4, 5, puan });

            return Ok(new
            {
                Kimlik = maskeliTc,
                Durum = "Doğrulandı",
                HotAlert = alarm,
                GenelMemnuniyetOrtalamasi = ortalama,
                SmsDurumu = "Gönderime Hazır"
            });
        }
        [HttpGet("tam-denetim-testi")]
        public IActionResult TamDenetimV2(string tcNo, string telefon, int puan)
        {
            // İşlem başında log alıyoruz
            var maskeli = _securityManager.TCKimlikMaskele(tcNo);
            _logger.LogAction("Anket Sorgulama", $"Kullanıcı: {maskeli} için denetim başlatıldı.");

            // ... (diğer kodlar aynı)

            return Ok(new { Message = "İşlem Loglandı", Loglar = _logger.GetLogs() as List<AuditLog> });
        }

        [HttpPost("toplu-cevap-kaydet")]
        public IActionResult SaveSurvey([FromBody] List<Response> cevaplar)
        {
            // Business katmanındaki ana motoru çağırıyoruz
            var sonuc = _anketManager.AnketTamamla(cevaplar);

            return Ok(sonuc);
        }
        private static List<Response> _cevaplar = new List<Response>();

        // Mock Data: Sistem açıldığında hazır gelecek sorular
        private static List<SoruBankasi> _soruBankasi = new List<SoruBankasi>
    {
        new SoruBankasi { Id = 1, Text = "Hastanemizin temizliğinden memnun musunuz?", IsCritical = true, Order = 1 },
        new SoruBankasi { Id = 2, Text = "Personel size güler yüzlü davrandı mı?", IsCritical = false, Order = 2 }
    };

        [HttpGet("sorulari-getir")]
        public IActionResult GetQuestions()
        {
            // Mock datayı dönüyoruz
            return Ok(_soruBankasi);
        }

        [HttpPost("cevap-kaydet")]
        public IActionResult SaveResponse([FromBody] Response response)
        {
            // Gelen JSON veriyi (Request Body) listeye ekliyoruz
            _cevaplar.Add(response);
            return Ok(new { Mesaj = "Cevap belleğe kaydedildi.", ToplamKayit = _cevaplar.Count });
        }

        [HttpPost("yeni-anket-linki-olustur")]
        public IActionResult CreateLink()
        {
            var token = _linkManager.CreateSurveyToken();
            var yeniAnket = new Survey
            {
                Id = _aktifAnketler.Count + 1,
                GuidToken = _linkManager.CreateSurveyToken(),
                ExpireDate = DateTime.Now.AddDays(2), // 48 saat kuralı
                IsUsed = false
            };
            _aktifAnketler.Add(yeniAnket);
            _logManager.AddLog("Token Üretildi", $"Sistemde yeni bir anket linki oluşturuldu. Token: {token}");

            return Ok(new
            {
                Mesaj = "Yeni anket başarıyla oluşturuldu.",
                AnketId = yeniAnket.Id,
                Token = yeniAnket.GuidToken,
                GecerlilikTarihi = yeniAnket.ExpireDate.ToString("dd.MM.yyyy HH:mm")
            });
        }

        [HttpPost("tokenli-cevap-kaydet")]
        public IActionResult SaveWithToken(string token, [FromBody] List<Response> cevaplar)
        {
            // Token geçerli mi?
            if (!_linkManager.IsTokenValid(token, _aktifAnketler))
                return BadRequest("Hata: Geçersiz veya süresi dolmuş anket linki!");

            // İşlemi tamamla ve token'ı kullanıldı olarak işaretle
            var anket = _aktifAnketler.First(s => s.GuidToken == token);
            anket.IsUsed = true;

           
            _logManager.AddLog("Anket Dolduruldu", $"Bir hasta, {token} anahtarını kullanarak anketini başarıyla tamamladı.");

            var validationHatasi = _validation.ValidateSurveyResponses(cevaplar);
            if (!string.IsNullOrEmpty(validationHatasi))
            {
                // Eğer kalkan bir hata tespit ederse, işlemi durdur ve kullanıcıya hatayı dön
                return BadRequest(new { Hata = validationHatasi });
            }

            // 3. İşleme Devam Et ve Kaydet (Mevcut kodların aynen kalacak)
            var mevcutanket = _aktifAnketler.First(s => s.GuidToken == token);
            anket.IsUsed = true;
            return Ok(new { Durum = "Başarılı", Mesaj = "Anketiniz kaydedildi." });
        }
        [HttpGet("tum-sonuclari-listele")]
        public IActionResult GetResults()
        {
            // Eğer bellekte hiç cevap yoksa boş döner, varsa hepsini listeler
            return Ok(_cevaplar);
        }

        [HttpGet("sistem-loglarini-getir")]
        public IActionResult GetSystemLogs()
        {
            return Ok(_logManager.GetLogs());
        }
        [HttpPost("token-dogrula")]
        public IActionResult TokenDogrula([FromBody] GirisBilgileri request)
        {
            // 1. Gelen Token boş mu kontrolü
            if (string.IsNullOrWhiteSpace(request.Token))
            {
                return BadRequest("Lütfen geçerli bir anket kodu (Token) giriniz.");
            }

            // 2. VERİTABANI KONTROLÜ (Kendi mimarine göre burayı güncelleyebilirsin)
            // Şimdilik test edebilmen için basit bir "Token uzunluğu" kontrolü koyuyorum
            if (request.Token.Length < 5)
            {
                return BadRequest("Token geçersiz veya süresi dolmuş.");
            }

            // Token başarılıysa 200 OK dön ve onay ver
            return Ok(new { Mesaj = "Doğrulama başarılı, ankete geçebilirsiniz." });
        }

        // JavaScript'ten gelen verileri C#'ta karşılayacak sepetimiz (Model)
        public class GirisBilgileri
        {
            public string Token { get; set; }
            public string HastaTuru { get; set; }
            public string AdSoyad { get; set; }
            public string TcKimlik { get; set; }
            public string DogumTarihi { get; set; }
            public string Telefon { get; set; }
            public string AnketiDolduran { get; set; }
            public string SosyalGuvence { get; set; }
            public string YatanServis { get; set; }
            public string AcilBasvuruSekli { get; set; }
            public string AcilBasvuruNedeni { get; set; }
        }
    }
}







