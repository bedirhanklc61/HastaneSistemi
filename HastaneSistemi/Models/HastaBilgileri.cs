using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneSistemi.Models
{
    [Table("Hastalar")]
    public class HastaBilgileri
    {
        public string AdSoyad { get; set; }

        [Key]
        public string TC { get; set; }

        public string Email { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string Sifre { get; set; }
        public string TemaModu { get; set; }
        public bool YaziBuyuk { get; set; }

    }

}
