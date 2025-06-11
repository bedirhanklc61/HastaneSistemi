using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneSistemi.Models
{
    [Table("Randevular")]
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
