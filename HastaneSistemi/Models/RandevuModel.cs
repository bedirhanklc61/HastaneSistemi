

namespace HastaneSistemi.Models
{
    public class RandevuModel
    {
        public int RandevuID { get; set; }
        public string HastaAdi { get; set; }
        public string Bolum { get; set; }
        public string Tarih { get; set; }
        public string Saat { get; set; }
        public int DoktorID { get; set; }
    }
}
