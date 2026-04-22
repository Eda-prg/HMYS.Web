using HMYS.Web.Features.AnketGonderim.Commands;
using HMYS.Web.Features.AnketGonderim.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HMYS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnketGonderimController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnketGonderimController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("token-uret")]
        public async Task<IActionResult> TokenUret([FromBody] TokenOlusturCommand command)
        {
            var token = await _mediator.Send(command);

            // Gerçek hayatta burada hastanın telefonuna SMS atma servisi devreye girer.
            // Biz şimdilik React ön yüzümüze tıklanacak bir link simülasyonu yapıyoruz:
            string hastaIcinLink = $"http://localhost:3000/anket?token={token}";

            return Ok(new
            {
                Mesaj = "Güvenli anket linki başarıyla üretildi!",
                Token = token,
                SimulasyonLink = hastaIcinLink
            });
        }

        [HttpGet("dogrula/{token}")]
        public async Task<IActionResult> TokenDogrula(string token)
        {
            var query = new TokenDogrulaQuery { Token = token };
            var sonuc = await _mediator.Send(query);

            if (!sonuc.GecerliMi)
            {
                // Güvenlikten geçemedi (Süresi bitmiş, kullanılmış vs.) - 400 Bad Request dön
                return BadRequest(new { Mesaj = sonuc.HataMesaji });
            }

            // Güvenlikten başarıyla geçti! 
            // React tarafına Hasta ve Randevu ID'sini ver ki anketi bu hasta adına SQL'e kaydedebilsin.
            return Ok(new
            {
                Mesaj = "Bağlantı güvenli. Anket sayfası açılıyor...",
                HastaId = sonuc.HastaId,
                RandevuId = sonuc.RandevuId
            });
        }
            [HttpPost("cevapla")]
            public async Task<IActionResult> AnketCevapla([FromBody] AnketCevaplaCommand command)
            {
                try
                {
                    var sonuc = await _mediator.Send(command);
                    return Ok(new { Mesaj = "Anket başarıyla kaydedildi! Katılımınız için teşekkürler." });
                }
                catch (Exception ex)
                {
                var gercekHata = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest(new { Hata = gercekHata });
            }
            }
        }
        }
    
