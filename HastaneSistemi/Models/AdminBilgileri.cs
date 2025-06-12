
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HastaneSistemi.Models
{
    [Table("Adminler")]
    public class AdminBilgileri
    {
        [Key]
        public int AdminID { get; set; }

        public string Email { get; set; }
        public string Sifre { get; set; }
    }
}
