using MediatR;
using System;
using System.Linq; // Toplama işlemi için eklendi
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HMYS.Web.Data;
using HMYS.Core.Entities;

namespace HMYS.Web.Features.AnketGonderim.Commands
{
    public class AnketCevaplaCommandHandler : IRequestHandler<AnketCevaplaCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public AnketCevaplaCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(AnketCevaplaCommand request, CancellationToken cancellationToken)
        {
            // 1. GÜVENLİK: Gönderimi Bul
            var gonderim = await _context.AnketGonderimleri
                .FirstOrDefaultAsync(x => x.TokenID == request.Token, cancellationToken);

            if (gonderim == null || gonderim.KullanildiMi)
            {
                throw new Exception("Hata: Bu anket bağlantısı geçersiz veya daha önce kullanılmış!");
            }

            // 2. ANA FİŞİ KES (MASTER KAYIT)
            // Hastanın verdiği tüm puanları otomatik topluyoruz
            var toplamPuan = request.Cevaplar.Sum(x => x.Puan);

            var masterKayit = new CevapMaster
            {
                GonderimID = gonderim.GonderimID,
                TamamlanmaTarihi = DateTime.Now,
                ToplamSkor = toplamPuan
            };

            // DİKKAT: DbContext içindeki DbSet ismin "CevapMasterlar" ise böyle bırak
            _context.CevapMaster.Add(masterKayit);

            // Veritabanına kaydedip yeni ResponseID'yi (Fiş No) alıyoruz
            await _context.SaveChangesAsync(cancellationToken);

            // 3. DETAYLARI FİŞE BAĞLA (DETAY KAYIT)
            foreach (var item in request.Cevaplar)
            {
                var detay = new CevapDetay
                {
                    ResponseID = masterKayit.ResponseID, // Artık oluşan gerçek Fiş Numarasını veriyoruz!
                    QuestionID = item.QuestionID,
                    Puan = item.Puan,
                    MetinCevap = item.MetinCevap
                };

                _context.CevapDetay.Add(detay);
            }

            // 4. TOKEN'I YAK VE SON KAYDI YAP
            gonderim.KullanildiMi = true;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}