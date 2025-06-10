using Microsoft.AspNetCore.Mvc;

namespace HastaneSistemi.Models
{
    public class DoktorBilgileri
    {
        public string AdSoyad { get; set; }
        public string Email { get; set; }
        public string Sifre { get; set; }
        public string TemaModu { get; set; }
    }

}
