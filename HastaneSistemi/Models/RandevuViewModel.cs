using System;
using Microsoft.AspNetCore.Mvc;

namespace HastaneSistemi.Models
{
    public class RandevuViewModel
    {
        public int RandevuID { get; set; }
        public DateTime Tarih { get; set; }
        public string Saat { get; set; }
        public string Bolum { get; set; }
        public string DoktorAd { get; set; }
        public string HastaAd { get; set; }
    }
}
