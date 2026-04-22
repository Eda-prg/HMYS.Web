using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using HMYS.Core.Entities;
using HMYS.Web.Data;

namespace HMYS.Web.Features.AnketGonderim.Queries
{
    public class TokenDogrulamaSonucu
    {
        public bool GecerliMi { get; set; }
        public string HataMesaji { get; set; }
        public int? HastaId { get; set; }
        public int? RandevuId { get; set; }
    }

    public class TokenDogrulaQuery : IRequest<TokenDogrulamaSonucu>
    {
        public string Token { get; set; }
    }

    // 3. İŞLEYİCİMİZ (Güvenlik Görevlisi Sınıfımız)
    public class TokenDogrulaQueryHandler : IRequestHandler<TokenDogrulaQuery, TokenDogrulamaSonucu>
    {
        private readonly ApplicationDbContext _context;

        public TokenDogrulaQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TokenDogrulamaSonucu> Handle(TokenDogrulaQuery request, CancellationToken cancellationToken)
        {
            // 1. Veritabanında bu token'ı ara
            var gonderimKaydi = await _context.AnketGonderimleri
                .FirstOrDefaultAsync(x => x.TokenID == request.Token, cancellationToken);

            // KURAL 1: Token veritabanında hiç yok mu?
            if (gonderimKaydi == null)
            {
                return new TokenDogrulamaSonucu { GecerliMi = false, HataMesaji = "Geçersiz veya sahte bir anket linki tıkladınız." };
            }

            // KURAL 2: Token daha önce kullanılmış mı?
            if (gonderimKaydi.KullanildiMi)
            {
                return new TokenDogrulamaSonucu { GecerliMi = false, HataMesaji = "Bu anket linki daha önce kullanılmış. Bir ankete sadece bir kez katılabilirsiniz." };
            }

            // KURAL 3: Token'ın süresi (7 gün) dolmuş mu?
            if (gonderimKaydi.SonKullanmaTarihi < DateTime.Now)
            {
                return new TokenDogrulamaSonucu { GecerliMi = false, HataMesaji = "Bu anketin geçerlilik süresi dolmuştur." };
            }

            // BÜTÜN KONTROLLERDEN GEÇTİ! (Token Geçerli)
            return new TokenDogrulamaSonucu
            {
                GecerliMi = true,
                HastaId = gonderimKaydi.Randevu?.HastaID,
                RandevuId = gonderimKaydi.RandevuID
            };
        }
    }
}