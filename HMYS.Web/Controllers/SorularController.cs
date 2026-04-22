using MediatR;
using Microsoft.AspNetCore.Mvc;
using HMYS.Web.Features.Sorular.Queries;
using HMYS.Web.Features.Sorular.Commands; // Yeni eklediğimiz Command sınıfının yolu

namespace HMYS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SorularController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SorularController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 1. OKUMA İŞLEMİ (Cache'den veya SQL'den listeyi getirir)
        [HttpGet]
        public async Task<IActionResult> GetTumSorular()
        {
            var sorular = await _mediator.Send(new GetTumSorularQuery());
            return Ok(sorular);
        }

        // 2. YAZMA İŞLEMİ (Yeni soru ekler ve eski Cache'i temizler)
        [HttpPost]
        public async Task<IActionResult> SoruEkle([FromBody] SoruEkleCommand command)
        {
            var yeniSoruId = await _mediator.Send(command);

            return Ok(new
            {
                Mesaj = "Soru başarıyla SQL'e eklendi ve Cache güncellendi!",
                EklenenSoruID = yeniSoruId
            });
        }
    }
}