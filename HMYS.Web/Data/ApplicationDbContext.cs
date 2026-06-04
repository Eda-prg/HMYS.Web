using HMYS.Core;
using HMYS.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HMYS.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Rol> Roller { get; set; }
        public DbSet<Birim> Birimler { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Doktor> Doktorlar { get; set; }
        public DbSet<Hasta> Hastalar { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<SoruTipi> SoruTipleri { get; set; }
        public DbSet<SoruBankasi> SoruBankasi { get; set; }
        public DbSet<Survey> AnketTanimlari { get; set; }
        public DbSet<AnketSoru> AnketSorulari { get; set; }
        public DbSet<AnketGonderim> AnketGonderimleri { get; set; }
        public DbSet<CevapMaster> CevapMaster { get; set; }
        public DbSet<CevapDetay> CevapDetay { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Entity'lerde [Column], [Table], [ForeignKey] attribute'ları zaten tanımlı
            // Burada tekrar yazmaya gerek yok
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) // ← EKLENDİ
            {
                optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
            }
        }
    }
}