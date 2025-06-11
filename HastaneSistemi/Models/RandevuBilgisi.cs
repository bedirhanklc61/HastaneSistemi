using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace HastaneSistemi.Models
{
    public class RandevuBilgisi
    {

        [Key]
        public int RandevuID { get; set; }


        public string TcKimlikNo { get; set; }
        public string Bolum { get; set; }
        public int DoktorID { get; set; }
        public DateTime Tarih { get; set; }
        public string Saat { get; set; }
    }

    




}
