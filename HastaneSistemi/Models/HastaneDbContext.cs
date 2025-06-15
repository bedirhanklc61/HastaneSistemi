
using Microsoft.EntityFrameworkCore;


namespace HastaneSistemi.Models
{
    public class HastaneDbContext : DbContext
    {
        public HastaneDbContext(DbContextOptions<HastaneDbContext> options)
            : base(options)
        {
        }

        public DbSet<Poliklinik> Poliklinikler { get; set; }
        public DbSet<DoktorBilgileri> DoktorBilgileri { get; set; }
        public DbSet<HastaBilgileri> Hastalar { get; set; }
        public DbSet<RandevuBilgisi> Randevular { get; set; }
        public DbSet<DoktorUygunluk> DoktorUygunluklar { get; set; }
        public DbSet<AdminBilgileri> Adminler { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Poliklinik>().HasData(
     new Poliklinik { PoliklinikID = 1, Ad = "Acil Tıp", Ikon = "uil-ambulance" },
     new Poliklinik { PoliklinikID = 2, Ad = "Aile Hekimliği", Ikon = "uil-user-md" },
     new Poliklinik { PoliklinikID = 3, Ad = "Anesteziyoloji ve Reanimasyon", Ikon = "uil-clinic-medical" },
     new Poliklinik { PoliklinikID = 4, Ad = "Beyin ve Sinir Cerrahisi", Ikon = "uil-brain" },
     new Poliklinik { PoliklinikID = 5, Ad = "Cildiye", Ikon = "uil-swatchbook" },
     new Poliklinik { PoliklinikID = 6, Ad = "Endokrinoloji ve Metabolizma Hastalıkları", Ikon = "uil-flask" },
     new Poliklinik { PoliklinikID = 7, Ad = "Enfeksiyon Hastalıkları", Ikon = "uil-virus-slash" },
     new Poliklinik { PoliklinikID = 8, Ad = "Fiziksel Tıp ve Rehabilitasyon", Ikon = "uil-wheelchair" },
     new Poliklinik { PoliklinikID = 9, Ad = "Gastroenteroloji", Ikon = "uil-stethoscope" },
     new Poliklinik { PoliklinikID = 10, Ad = "Genel Cerrahi", Ikon = "uil-hospital" },
     new Poliklinik { PoliklinikID = 11, Ad = "Genel Dahiliye", Ikon = "uil-medical-drip" },
     new Poliklinik { PoliklinikID = 12, Ad = "Girişimsel Radyoloji", Ikon = "uil-flask" },
     new Poliklinik { PoliklinikID = 13, Ad = "Göz Hastalıkları", Ikon = "uil-eye" },
     new Poliklinik { PoliklinikID = 14, Ad = "Göğüs Cerrahisi", Ikon = "uil-stethoscope" },
     new Poliklinik { PoliklinikID = 15, Ad = "Göğüs Hastalıkları", Ikon = "uil-stethoscope" },
     new Poliklinik { PoliklinikID = 16, Ad = "Hematoloji", Ikon = "uil-syringe" },
     new Poliklinik { PoliklinikID = 17, Ad = "Kadin Hastalıkları", Ikon = "uil-user" },
     new Poliklinik { PoliklinikID = 18, Ad = "Kalp ve Damar Cerrahisi", Ikon = "uil-heart" },
     new Poliklinik { PoliklinikID = 19, Ad = "Kardiyoloji", Ikon = "uil-heartbeat" },
     new Poliklinik { PoliklinikID = 20, Ad = "Kulak Burun Boğaz", Ikon = "uil-headphones" },
     new Poliklinik { PoliklinikID = 21, Ad = "Nefroloji", Ikon = "uil-kid" },
     new Poliklinik { PoliklinikID = 22, Ad = "Nöroloji", Ikon = "uil-brain" },
     new Poliklinik { PoliklinikID = 23, Ad = "Nükleer Tıp", Ikon = "uil-atom" },
     new Poliklinik { PoliklinikID = 24, Ad = "Ortopedi", Ikon = "uil-wheelchair" },
     new Poliklinik { PoliklinikID = 25, Ad = "Plastik, Rekonstrüktif ve Estetik Cerrahi", Ikon = "uil-user-check" },
     new Poliklinik { PoliklinikID = 26, Ad = "Radyasyon Onkolojisi", Ikon = "uil-atom" },
     new Poliklinik { PoliklinikID = 27, Ad = "Radyoloji", Ikon = "uil-dna" },
     new Poliklinik { PoliklinikID = 28, Ad = "Romatoloji", Ikon = "uil-wheelchair" },
     new Poliklinik { PoliklinikID = 29, Ad = "Ruh Sağlığı ve Hastalıkları", Ikon = "uil-user" },
     new Poliklinik { PoliklinikID = 30, Ad = "Tibbi Biyokimya", Ikon = "uil-flask" },
     new Poliklinik { PoliklinikID = 31, Ad = "Tibbi Genetik", Ikon = "uil-dna" },
     new Poliklinik { PoliklinikID = 32, Ad = "Tibbi Mikrobiyoloji", Ikon = "uil-virus-slash" },
     new Poliklinik { PoliklinikID = 33, Ad = "Tibbi Onkoloji", Ikon = "uil-dna" },
     new Poliklinik { PoliklinikID = 34, Ad = "Tibbi Patoloji", Ikon = "uil-notes" },
     new Poliklinik { PoliklinikID = 35, Ad = "Yeni Doğan", Ikon = "uil-baby-carriage" },
     new Poliklinik { PoliklinikID = 36, Ad = "Çocuk Acil", Ikon = "uil-ambulance" },
     new Poliklinik { PoliklinikID = 37, Ad = "Çocuk Cerrahisi", Ikon = "uil-syringe" },
     new Poliklinik { PoliklinikID = 38, Ad = "Çocuk Endokrinolojisi", Ikon = "uil-baby-carriage" },
     new Poliklinik { PoliklinikID = 39, Ad = "Çocuk Enfeksiyon ve Hastalıkları", Ikon = "uil-virus-slash" },
     new Poliklinik { PoliklinikID = 40, Ad = "Çocuk Gastroenterolojisi", Ikon = "uil-stethoscope-alt" },
     new Poliklinik { PoliklinikID = 41, Ad = "Çocuk Hematolojisi ve Onkolojisi", Ikon = "uil-dna" },
     new Poliklinik { PoliklinikID = 42, Ad = "Çocuk Kardiyolojisi", Ikon = "uil-heart" },
     new Poliklinik { PoliklinikID = 43, Ad = "Çocuk Nefrolojisi", Ikon = "uil-kid" },
     new Poliklinik { PoliklinikID = 44, Ad = "Çocuk Nörolojisi", Ikon = "uil-brain" },
     new Poliklinik { PoliklinikID = 45, Ad = "Çocuk Romatolojisi", Ikon = "uil-wheelchair-alt" },
     new Poliklinik { PoliklinikID = 46, Ad = "Çocuk Sağlığı ve Hastalıkları", Ikon = "uil-kid" },
     new Poliklinik { PoliklinikID = 47, Ad = "Çocuk ve Ergen Ruh Sağlığı", Ikon = "uil-user" },
     new Poliklinik { PoliklinikID = 48, Ad = "Çocuk İmmünolojisi ve Alerji Hastalıkları", Ikon = "uil-syringe" },
     new Poliklinik { PoliklinikID = 49, Ad = "Üroloji", Ikon = "uil-medkit" },
     new Poliklinik { PoliklinikID = 50, Ad = "İmmünoloji ve Alerji Hastalıkları", Ikon = "uil-syringe" }
 );

        }

    }
}

