using HMYS.Core;
using HMYS.Core.Entities;
using HMYS.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace HMYS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SorularController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache; // Redis arayüzümüz

        // Dependency Injection ile hem SQL'i hem de Redis'i Controller'a alıyoruz
        public SorularController(ApplicationDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        // GET: api/Sorular
        [HttpGet]
        public async Task<IActionResult> GetTumSorular()
        {
            string cacheKey = "anket_sorulari_listesi"; // Redis'te bu veriyi bulacağımız anahtar kelime

            // 1. ADIM: Önce Redis'e sor (Cache'de var mı?)
            var cachedSorular = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedSorular))
            {
                Console.WriteLine("-----> DİKKAT: Veriler CACHE'DEN (Sanal Redis) çok hızlı çekildi! Veritabanı yorulmadı.");
                // REDIS'TE BULDUK! (Cache Hit)
                // Veritabanına hiç gitmeden, SQL'i hiç yormadan doğrudan JSON'u listeye çevirip döndür
                var sorularFromCache = JsonSerializer.Deserialize<List<SoruBankasi>>(cachedSorular);
                return Ok(sorularFromCache);
            }
            Console.WriteLine("-----> DİKKAT: Cache boş! Veriler mecburen SQL VERİTABANINDAN çekiliyor...");
            
            var sorularFromDb = await _context.SoruBankasi.ToListAsync();

            var cacheOptions = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); // Bu veri 10 dakika boyunca Redis'te yaşasın

            string serializedSorular = JsonSerializer.Serialize(sorularFromDb);
            await _cache.SetStringAsync(cacheKey, serializedSorular, cacheOptions);

            // Veriyi kullanıcıya döndür
            return Ok(sorularFromDb);
        }

        // POST metodu aynen kalabilir...
        [HttpPost]
        public async Task<IActionResult> SoruEkle([FromBody] SoruBankasi yeniSoru)
        {
            _context.SoruBankasi.Add(yeniSoru);
            await _context.SaveChangesAsync();
            return Ok(new { Mesaj = "Soru başarıyla eklendi!" });
        }
    }
}