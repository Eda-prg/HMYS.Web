using HMYS.Core.Entities;
using HMYS.Web.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HMYS.Web.Features.AnketGonderim.Commands
{
    public class TokenOlusturCommand : IRequest<string>
    {
        public int HastaID { get; set; }
        public int RandevuID { get; set; }
    }

    // 2. İŞLEYİCİMİZ (Token'ı üretip SQL'e kaydeden işçi sınıf)
    public class TokenOlusturCommandHandler : IRequestHandler<TokenOlusturCommand, string>
    {


        private readonly ApplicationDbContext _context;

        public TokenOlusturCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> Handle(TokenOlusturCommand request, CancellationToken cancellationToken)
        {
            var debugSorgusu = _context.Randevular.ToQueryString();
            Console.WriteLine(debugSorgusu);
            try
            {
                string benzersizToken = Guid.NewGuid().ToString();

                var yeniGonderim = new HMYS.Core.Entities.AnketGonderim
                {
                    // EĞER SQL tablanda HastaID sütunu yoksa BU SATIRI TAMAMEN SİL:
                    HastaID = request.HastaID,

                    RandevuID = request.RandevuID,
                    TokenID = benzersizToken,
                    SonKullanmaTarihi = DateTime.Now.AddDays(7),
                    KullanildiMi = false
                };

                _context.AnketGonderimleri.Add(yeniGonderim);
                await _context.SaveChangesAsync(cancellationToken);

                return benzersizToken;
            }
            catch (Exception ex)
            {
                var realError = ex.InnerException?.Message ?? ex.Message;
                throw new Exception("SQL Kayıt Hatası: " + realError);
            }
        }
    }
}