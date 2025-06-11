using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

    }
}

