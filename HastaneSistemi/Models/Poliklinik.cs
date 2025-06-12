using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneSistemi.Models
{
    [Table("Poliklinikler")]
    public class Poliklinik
    {
        [Key]
        public int PoliklinikID { get; set; }

        public string Ad { get; set; }
        public string Ikon { get; set; }
    }
}
