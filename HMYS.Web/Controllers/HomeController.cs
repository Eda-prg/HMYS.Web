using HMYS.Business;
using HMYS.Business.Interfaces;  // ← EKLENDİ
using HMYS.Core;
using Microsoft.AspNetCore.Mvc;

namespace HMYS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly AnketManager _anketManager;
        private readonly SecurityManager _securityManager = new SecurityManager();
        private readonly LinkManager _linkManager = new LinkManager();
        private readonly ValidationManager _validation = new ValidationManager();
        private readonly AnalyticsManager _analytics = new AnalyticsManager();
        private readonly LogManager _logger = new LogManager();
        private static List<Survey> _aktifAnketler = new List<Survey>();
        private readonly LogManager _logManager = new LogManager();
        private readonly IEmailService _emailService; // ← EKLENDİ

        // ← CONSTRUCTOR EKLENDİ
        public HomeController(IEmailService emailService)
        {
            _emailService = emailService;
            _anketManager = new AnketManager(_analytics, _logger);
        }

        [HttpGet("test-hmys")]
        public IActionResult TestHmys(string tcNo, int puan, bool kritikSoru)
        {
            var maskeliTc = _securityManager.TCKimlikMaskele(tcNo);
            var alarmVarMi = _anketManager.HotAlertKontrol(puan, kritikSoru);
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
            var token = _linkManager.GenerateSecureToken();
            var sonTarih = _linkManager.GetExpirationDate();
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
            if (!_validation.IsValidTc(tcNo)) return BadRequest("Geçersiz TC Kimlik formatı!");
            if (!_validation.IsValidPhone(telefon)) return BadRequest("Geçersiz Telefon formatı!");
            var maskeliTc = _securityManager.TCKimlikMaskele(tcNo);
            var alarm = _anketManager.HotAlertKontrol(puan, true);
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
            var maskeli = _securityManager.TCKimlikMaskele(tcNo);
            _logger.LogAction("Anket Sorgulama", $"Kullanıcı: {maskeli} için denetim başlatıldı.");
            return Ok(new { Message = "İşlem Loglandı", Loglar = _logger.GetLogs() as List<AuditLog> });
        }

        [HttpPost("toplu-cevap-kaydet")]
        public IActionResult SaveSurvey([FromBody] List<Response> cevaplar)
        {
            var sonuc = _anketManager.AnketTamamla(cevaplar);
            return Ok(sonuc);
        }

        private static List<Response> _cevaplar = new List<Response>();

        private static List<SoruBankasi> _soruBankasi = new List<SoruBankasi>
        {
            new SoruBankasi { Id = 1, Text = "Hastanemizin temizliğinden memnun musunuz?", IsCritical = true, Order = 1 },
            new SoruBankasi { Id = 2, Text = "Personel size güler yüzlü davrandı mı?", IsCritical = false, Order = 2 }
        };

        [HttpGet("sorulari-getir")]
        public IActionResult GetQuestions()
        {
            return Ok(_soruBankasi);
        }

        [HttpPost("cevap-kaydet")]
        public IActionResult SaveResponse([FromBody] Response response)
        {
            _cevaplar.Add(response);
            return Ok(new { Mesaj = "Cevap belleğe kaydedildi.", ToplamKayit = _cevaplar.Count });
        }

        // ← async Task<IActionResult> YAPILDI + dto PARAMETRESİ EKLENDİ
        [HttpPost("yeni-anket-linki-olustur")]
        public async Task<IActionResult> CreateLink([FromBody] AnketLinkDto dto)
        {
            var token = _linkManager.CreateSurveyToken();
            var yeniAnket = new Survey
            {
                Id = _aktifAnketler.Count + 1,
                GuidToken = token,
                ExpireDate = DateTime.Now.AddDays(2),
                IsUsed = false
            };
            _aktifAnketler.Add(yeniAnket);
            _logManager.AddLog("Token Üretildi", $"Token: {token}");

            // ← E-POSTA GÖNDERME
            var basarili = await _emailService.SendSurveyLinkAsync(
                toEmail: dto.HastaEmail,
                hastaAdi: dto.HastaAdi,
                token: token
            );

            return Ok(new
            {
                Mesaj = basarili
                    ? "Anket oluşturuldu ve e-posta gönderildi."
                    : "Anket oluşturuldu ama e-posta gönderilemedi.",
                AnketId = yeniAnket.Id,
                Token = token,
                GecerlilikTarihi = yeniAnket.ExpireDate.ToString("dd.MM.yyyy HH:mm"),
                EpostaGonderildi = basarili
            });
        }

        [HttpPost("tokenli-cevap-kaydet")]
        public IActionResult SaveWithToken(string token, [FromBody] List<Response> cevaplar)
        {
            if (!_linkManager.IsTokenValid(token, _aktifAnketler))
                return BadRequest("Hata: Geçersiz veya süresi dolmuş anket linki!");

            var anket = _aktifAnketler.First(s => s.GuidToken == token);
            anket.IsUsed = true;

            _logManager.AddLog("Anket Dolduruldu", $"Token: {token} ile anket tamamlandı.");

            var validationHatasi = _validation.ValidateSurveyResponses(cevaplar);
            if (!string.IsNullOrEmpty(validationHatasi))
                return BadRequest(new { Hata = validationHatasi });

            return Ok(new { Durum = "Başarılı", Mesaj = "Anketiniz kaydedildi." });
        }

        [HttpGet("tum-sonuclari-listele")]
        public IActionResult GetResults()
        {
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
            if (string.IsNullOrWhiteSpace(request.Token))
                return BadRequest("Lütfen geçerli bir anket kodu (Token) giriniz.");

            if (request.Token.Length < 5)
                return BadRequest("Token geçersiz veya süresi dolmuş.");

            return Ok(new { Mesaj = "Doğrulama başarılı, ankete geçebilirsiniz." });
        }

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

        // ← DTO SINIFI EKLENDİ
        public class AnketLinkDto
        {
            public string HastaEmail { get; set; }
            public string HastaAdi { get; set; }
        }
    }
}