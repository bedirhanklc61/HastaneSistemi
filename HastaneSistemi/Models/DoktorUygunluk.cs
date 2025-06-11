using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace HastaneSistemi.Models
{
    public class DoktorUygunluk
    {
        [Key]
        public int UygunlukID { get; set; }

        public int DoktorID { get; set; }
        public DateTime BaslangicTarih { get; set; }
        public DateTime BitisTarih { get; set; }
        public TimeSpan BaslangicSaat { get; set; }
        public TimeSpan BitisSaat { get; set; }
    }
}
