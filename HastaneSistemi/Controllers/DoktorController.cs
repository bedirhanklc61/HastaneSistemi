using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System;
using HastaneSistemi.Models;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;



namespace HastaneSistemi.Controllers
{
    public class DoktorController : Controller
    {
        private readonly string _connectionString;

        public DoktorController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HastaneDB");
        }

        public IActionResult DoktorPanel()
        {
            int? doktorID = HttpContext.Session.GetInt32("DoktorID");
            if (doktorID == null)
                return RedirectToAction("Index", "Login");

            var randevular = new List<RandevuModel>();
            var bugun = DateTime.Now;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Tema
                string email = HttpContext.Session.GetString("Email");
                var temaCmd = new SqlCommand("SELECT TemaModu FROM Doktorlar WHERE Email = @Email", conn);
                temaCmd.Parameters.AddWithValue("@Email", email);
                var temaDr = temaCmd.ExecuteReader();
                if (temaDr.Read()) ViewBag.TemaModu = temaDr["TemaModu"].ToString();
                temaDr.Close();

                // Randevular
                var cmd = new SqlCommand(@"
            SELECT r.RandevuID, r.Tarih, r.Saat, r.Bolum, h.AdSoyad AS HastaAdi
            FROM Randevular r
            INNER JOIN Hastalar h ON r.TcKimlikNo = h.TC
            WHERE r.DoktorID = @doktorID", conn);
                cmd.Parameters.AddWithValue("@doktorID", doktorID);

                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    randevular.Add(new RandevuModel
                    {
                        RandevuID = Convert.ToInt32(dr["RandevuID"]),
                        Tarih = Convert.ToDateTime(dr["Tarih"]).ToString("yyyy-MM-dd"),
                        Saat = dr["Saat"].ToString(),
                        Bolum = dr["Bolum"].ToString(),
                        HastaAdi = dr["HastaAdi"].ToString()
                    });
                }
            }

            // Ayır: geçmiş ve gelecek
            var gecmis = randevular
                .Where(r => DateTime.Parse(r.Tarih + " " + r.Saat) < DateTime.Now)
                .OrderByDescending(r => DateTime.Parse(r.Tarih + " " + r.Saat))
                .ToList();

            var gelecek = randevular
                .Where(r => DateTime.Parse(r.Tarih + " " + r.Saat) >= DateTime.Now)
                .OrderBy(r => DateTime.Parse(r.Tarih + " " + r.Saat))
                .ToList();

            ViewBag.GecmisRandevular = gecmis;
            var simdi = DateTime.Now;
            var sadeceGelecekler = randevular
                .Where(r => DateTime.Parse(r.Tarih + " " + r.Saat) >= simdi)
                .ToList();

            ViewBag.TestMesaj = "Yaklaşan randevu: " + sadeceGelecekler.Count;
            ViewBag.AdSoyad = HttpContext.Session.GetString("AdSoyad");

            return View(gelecek); // Model olarak sadece gelecek randevular gider
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RandevuIptalEt([FromBody] int randevuID)
        {
            if (randevuID <= 0)
                return BadRequest("Geçersiz randevu ID.");

            int? doktorID = HttpContext.Session.GetInt32("DoktorID");
            if (doktorID == null)
                return Unauthorized();


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var kontrolCmd = new SqlCommand("SELECT COUNT(*) FROM Randevular WHERE RandevuID = @id AND DoktorID = @dId", conn);
                kontrolCmd.Parameters.AddWithValue("@id", randevuID);
                kontrolCmd.Parameters.AddWithValue("@dId", doktorID.Value);
                int sahipMi = (int)kontrolCmd.ExecuteScalar();

                if (sahipMi == 0)
                    return Unauthorized();

                SqlCommand cmd = new SqlCommand("DELETE FROM Randevular WHERE RandevuID = @id", conn);
                cmd.Parameters.AddWithValue("@id", randevuID);
                int sonuc = cmd.ExecuteNonQuery();

                if (sonuc > 0)
                    return Ok();
                else
                    return BadRequest("Randevu silinemedi.");
            }
        }





        // Doktorlar API'si isteniyorsa bu da kalabilir:
        [HttpGet("/api/doktorlar")]
        public IActionResult GetDoktorlar()
        {
            List<object> doktorlar = new List<object>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT DoktorID, AdSoyad, Bolum FROM Doktorlar", conn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    doktorlar.Add(new
                    {
                        DoktorID = dr["DoktorID"],
                        AdSoyad = dr["AdSoyad"].ToString(),
                        Bolum = dr["Bolum"].ToString()
                    });
                }
            }

            return Ok(doktorlar);
        }

        [HttpGet]
        public IActionResult UygunlukAyarla()
        {
            int? doktorID = HttpContext.Session.GetInt32("DoktorID");
            if (doktorID == null)
                return RedirectToAction("Index", "Login");

            // Varsayılan model ile View’a gönder
            var model = new DoktorUygunluk
            {
                DoktorID = doktorID.Value,
                BaslangicTarih = DateTime.Today,
                BitisTarih = DateTime.Today.AddDays(1),
                BaslangicSaat = new TimeSpan(9, 0, 0),
                BitisSaat = new TimeSpan(17, 0, 0)
            };
            return View(model);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UygunlukAyarla(DoktorUygunluk model)
        {
            int? doktorID = HttpContext.Session.GetInt32("DoktorID");
            if (doktorID == null)
                return RedirectToAction("Index", "Login");

            model.DoktorID = doktorID.Value;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
            INSERT INTO DoktorUygunluk
              (DoktorID, BaslangicTarih, BitisTarih, BaslangicSaat, BitisSaat)
            VALUES
              (@d, @bT, @eT, @bS, @eS)", conn);
                cmd.Parameters.AddWithValue("@d", doktorID);
                cmd.Parameters.AddWithValue("@bT", model.BaslangicTarih);
                cmd.Parameters.AddWithValue("@eT", model.BitisTarih);
                cmd.Parameters.AddWithValue("@bS", model.BaslangicSaat);
                cmd.Parameters.AddWithValue("@eS", model.BitisSaat);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("DoktorPanel");
        }

        public IActionResult Ayarlar(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            // Sessiondan email alıp veritabanından doktoru çekelim
            string email = HttpContext.Session.GetString("Email");

            DoktorBilgileri doktor = null;
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT AdSoyad, Email, Sifre, TemaModu FROM Doktorlar WHERE Email = @Email", conn);
                cmd.Parameters.AddWithValue("@Email", email);
                var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    doktor = new DoktorBilgileri
                    {
                        AdSoyad = dr["AdSoyad"].ToString(),
                        Email = dr["Email"].ToString(),
                        Sifre = string.Empty,
                        TemaModu = dr["TemaModu"].ToString()
                    };
                }
            }

            return View(doktor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AyarlarGuncelle(DoktorBilgileri model)
        {
            string email = HttpContext.Session.GetString("Email");

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Doktorlar SET Sifre = @sifre, TemaModu = @tema WHERE Email = @Email", conn);
                var hasher = new PasswordHasher<DoktorBilgileri>();
                string hashed = hasher.HashPassword(null, model.Sifre);
                cmd.Parameters.AddWithValue("@sifre", hashed);
                cmd.Parameters.AddWithValue("@tema", model.TemaModu);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Ayarlar", new { returnUrl = "/Doktor/DoktorPanel" });
        }





    }
}
