using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HastaneSistemi.Models
{
    public class DoktorBilgileri

    {
        [Key]   
        public int DoktorID { get; set; }
        public string AdSoyad { get; set; }
        public string Email { get; set; }
        public string Sifre { get; set; }
        public string TemaModu { get; set; }
        public string Bolum { get; set; } 

    }

}
