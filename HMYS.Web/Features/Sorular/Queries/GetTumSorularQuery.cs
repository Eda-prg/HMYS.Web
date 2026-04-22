using HMYS.Core;
using HMYS.Core.Entities;
// DbContext'in bulunduğu yer (Senin Data klasörüne göre)
using HMYS.Web.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HMYS.Web.Features.Sorular.Queries
{
    public class GetTumSorularQuery : IRequest<List<SoruBankasi>>
    {
    }
    public class GetTumSorularQueryHandler : IRequestHandler<GetTumSorularQuery, List<SoruBankasi>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;

        public GetTumSorularQueryHandler(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }
        public async Task<List<SoruBankasi>> Handle(GetTumSorularQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = "anket_sorulari_listesi";
            var cachedSorular = await _cache.GetStringAsync(cacheKey, cancellationToken);

            if (!string.IsNullOrEmpty(cachedSorular))
            {
                Console.WriteLine("CQRS & MediatR: Veriler CACHE'DEN geldi!");
                return JsonSerializer.Deserialize<List<SoruBankasi>>(cachedSorular);
            }

            Console.WriteLine("CQRS & MediatR: Cache boş, SQL'den çekiliyor...");
            var sorularFromDb = await _context.SoruBankasi.ToListAsync(cancellationToken);

            var cacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(sorularFromDb), cacheOptions, cancellationToken);

            return sorularFromDb;
        }

    }
}
    

