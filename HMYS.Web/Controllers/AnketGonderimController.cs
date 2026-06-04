using HMYS.Web.Features.AnketGonderim.Commands;
using HMYS.Web.Features.AnketGonderim.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HMYS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnketGonderimController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration; // ← EKLENDİ

        public AnketGonderimController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration; // ← EKLENDİ
        }

        [HttpPost("token-uret")]
        public async Task<IActionResult> TokenUret([FromBody] TokenOlusturCommand command)
        {
            var token = await _mediator.Send(command);
            string frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000"; // ← DÜZELTİLDİ
            string hastaIcinLink = $"{frontendUrl}/anket?token={token}";

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
                return BadRequest(new { Mesaj = sonuc.HataMesaji });

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
            var sonuc = await _mediator.Send(command); // try/catch kaldırıldı — global handler'a taşı
            return Ok(new { Mesaj = "Anket başarıyla kaydedildi! Katılımınız için teşekkürler." });
        }
    }
}