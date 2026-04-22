using HMYS.Core;
using HMYS.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HMYS.Web.Data
{
    public class ApplicationDbContext : DbContext
    {

        // Kurucu metot (Program.cs'den bağlantı ayarlarını almak için)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Veritabanında oluşacak tablolarımızın listesi (DbSet'ler)
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
         
            modelBuilder.Entity<Randevu>(entity =>
            {
                entity.ToTable("randevular"); // Tablo adını kontrol et (R büyük/küçük?)

                // Bu satır kritik: "Kodda HastaID gördüğünde SQL'de HastaID kolonuna bak" diyoruz.
                entity.Property(e => e.HastaID).HasColumnName("HastaID");
                entity.Property(e => e.RandevuID).HasColumnName("RandevuID");
                entity.Property(e => e.DoktorID).HasColumnName("DoktorID");

                // İlişkiyi de burada elinle bağla (Hata ihtimalini sıfıra indirir)
                entity.HasOne(d => d.Hasta)
                      .WithMany(p => p.Randevular)
                      .HasForeignKey(d => d.HastaID)
                      .IsRequired(false);
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Debug penceresine SQL sorgularını yazdırır
            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
        }
    }
}
