using System;
using Microsoft.AspNetCore.Mvc;

namespace HastaneSistemi.Models
{
    public class HastaBilgileri
    {
        public string AdSoyad { get; set; }
        public string TC { get; set; }
        public string Email { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string Sifre { get; set; }
        public string TemaModu { get; set; }
        public bool YaziBuyuk { get; set; }

    }

}
