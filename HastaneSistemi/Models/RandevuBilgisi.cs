using System;
using Microsoft.AspNetCore.Mvc;

namespace HastaneSistemi.Models
{
    public class RandevuBilgisi
    {

        public string TcKimlikNo { get; set; }
        public string Bolum { get; set; }
        public int DoktorID { get; set; }
        public DateTime Tarih { get; set; }
        public string Saat { get; set; }
    }

    




}
