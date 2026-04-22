using HMYS.Core;
using HMYS.Core.Entities;
using HMYS.Web.Data;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HMYS.Web.Features.Sorular.Commands
{
    public class SoruEkleCommand : IRequest<int>
    {
        public string SoruMetniTR { get; set; }
        public int SoruTipiID { get; set; }
        public bool KritikSoruMu { get; set; }
    }
    public class SoruEkleCommandHandler : IRequestHandler<SoruEkleCommand, int>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;

        public SoruEkleCommandHandler(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<int> Handle(SoruEkleCommand request, CancellationToken cancellationToken)
        {
            // 1. Dışarıdan gelen verilerle yeni soruyu oluştur
            var yeniSoru = new SoruBankasi
            {
                // Not: Senin SoruBankasi tablosundaki özellik adların neyse onları kullan
                Text = request.SoruMetniTR,
                Order = request.SoruTipiID,
                IsCritical = request.KritikSoruMu
            };

            // 2. SQL Veritabanına kaydet
            _context.SoruBankasi.Add(yeniSoru);
            await _context.SaveChangesAsync(cancellationToken);

            // 3. EN ÖNEMLİ KISIM (CACHE INVALIDATION): Eski önbelleği sil!
            // Böylece bir sonraki GET isteğinde sistem mecbur gidip yeni listeyi SQL'den çekecek.
            await _cache.RemoveAsync("anket_sorulari_listesi", cancellationToken);
            Console.WriteLine("CQRS & MediatR: Yeni soru eklendi ve eski Cache temizlendi!");

            // 4. Yeni oluşan kimlik numarasını (ID) döndür (Senin sınıftaki Id veya QuestionID hangisiyse)
            return yeniSoru.Id;
        }
    }
}

