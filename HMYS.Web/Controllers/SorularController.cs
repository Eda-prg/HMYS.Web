using HMYS.Business.Interfaces;
using HMYS.Business.Services;
using HMYS.Web.Features.Sorular.Commands; // Yeni eklediğimiz Command sınıfının yolu
using HMYS.Web.Features.Sorular.Queries;
using HMYS.Web.Features.Sorular.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HMYS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SorularController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;


        public SorularController(IMediator mediator, IEmailService emailService)
        {
            _mediator = mediator;
            _emailService = emailService;
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
            [HttpPost("taburcu-bildir")]
            public async Task<IActionResult> TaburcuBildir([FromBody] TaburcuDto dto)
            {
                // 1. Token üret
                var token = Guid.NewGuid().ToString();

                // 2. Veritabanına kaydet (senin mevcut servisin)
                // await _tokenService.KaydetAsync(token, dto.SurveyId);

                // 3. E-posta gönder
                var basarili = await _emailService.SendSurveyLinkAsync(
                    toEmail: dto.HastaEmail,
                    hastaAdi: dto.HastaAdi,
                    token: token
                );

                if (basarili)
                    return Ok(new { mesaj = "E-posta gönderildi", token });
                else
                    return StatusCode(500, "E-posta gönderilemedi");
            }
        }
    }
