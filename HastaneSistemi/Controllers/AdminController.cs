using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HastaneSistemi.Models;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;


namespace HastaneSistemi.Controllers
{
    public class AdminController : Controller
    {
        private readonly string _connectionString;

        public AdminController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("HastaneDB");
        }

        public IActionResult AdminPanel()
        {
            List<Poliklinik> poliklinikler = new List<Poliklinik>();
            List<RandevuViewModel> randevular = new List<RandevuViewModel>();
            List<DoktorBilgileri> doktorlar = new List<DoktorBilgileri>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Poliklinikler
                SqlCommand cmd1 = new SqlCommand("SELECT * FROM Poliklinikler", conn);
                SqlDataReader dr1 = cmd1.ExecuteReader();
                while (dr1.Read())
                {
                    poliklinikler.Add(new Poliklinik
                    {
                        PoliklinikID = Convert.ToInt32(dr1["PoliklinikID"]),
                        Ad = dr1["Ad"].ToString(),
                        Ikon = dr1["Ikon"].ToString()
                    });
                }
                dr1.Close();

                // Randevular
                SqlCommand cmd2 = new SqlCommand("SELECT * FROM Randevular", conn);
                SqlDataReader dr2 = cmd2.ExecuteReader();
                while (dr2.Read())
                {
                    int doktorID = Convert.ToInt32(dr2["DoktorID"]);
                    string tc = dr2["TcKimlikNo"].ToString();

                    string doktorAd = GetDoktorAdi(doktorID);
                    string hastaAd = GetHastaAdi(tc);

                    randevular.Add(new RandevuViewModel
                    {
                        RandevuID = Convert.ToInt32(dr2["RandevuID"]),
                        Tarih = Convert.ToDateTime(dr2["Tarih"]),
                        Saat = dr2["Saat"].ToString(),
                        Bolum = dr2["Bolum"].ToString(),
                        DoktorAd = doktorAd,
                        HastaAd = hastaAd
                    });
                }
                dr2.Close();

                // ✅ Doktorlar
                SqlCommand cmd3 = new SqlCommand("SELECT * FROM Doktorlar", conn);
                SqlDataReader dr3 = cmd3.ExecuteReader();
                while (dr3.Read())
                {
                    doktorlar.Add(new DoktorBilgileri
                    {
                        DoktorID = Convert.ToInt32(dr3["DoktorID"]),
                        AdSoyad = dr3["AdSoyad"].ToString(),
                        Email = dr3["Email"].ToString(),
                        Bolum = dr3["Bolum"].ToString()
                    });
                }
                dr3.Close();
            }

            ViewBag.Poliklinikler = poliklinikler;
            ViewBag.Randevular = randevular;
            ViewBag.Doktorlar = doktorlar; 

            return View();
        }



        private string GetDoktorAdi(int doktorID)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT AdSoyad FROM Doktorlar WHERE DoktorID = @p1", conn);
                cmd.Parameters.AddWithValue("@p1", doktorID);
                return cmd.ExecuteScalar()?.ToString() ?? "Bilinmiyor";
            }
        }

        private string GetHastaAdi(string tcKimlik)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT AdSoyad FROM Hastalar WHERE TC = @tc", conn);
                cmd.Parameters.AddWithValue("@tc", tcKimlik);
                return cmd.ExecuteScalar()?.ToString() ?? "Bilinmiyor";
            }
        }

        [HttpPost]
        public IActionResult DoktorEkle(IFormCollection form)
        {
            string adSoyad = form["AdSoyad"];
            string email = form["Email"];
            string sifre = form["Sifre"];
            int poliklinikID = int.Parse(form["Bolum"]);

            string bolumAdi = "";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Poliklinik adını ID'den al
                SqlCommand cmd1 = new SqlCommand("SELECT Ad FROM Poliklinikler WHERE PoliklinikID = @id", conn);
                cmd1.Parameters.AddWithValue("@id", poliklinikID);
                bolumAdi = cmd1.ExecuteScalar()?.ToString() ?? "";

                if (!string.IsNullOrEmpty(bolumAdi))
                {
                    // Doktor kaydet
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO Doktorlar (AdSoyad, Bolum, Email, Sifre, TemaModu) VALUES (@p1, @p2, @p3, @p4, @p5)", conn);
                    var hasher = new PasswordHasher<DoktorBilgileri>();
                    string hashed = hasher.HashPassword(null, sifre);
                    cmd2.Parameters.AddWithValue("@p1", adSoyad);
                    cmd2.Parameters.AddWithValue("@p2", bolumAdi);
                    cmd2.Parameters.AddWithValue("@p3", email);
                    cmd2.Parameters.AddWithValue("@p4", hashed);
                    cmd2.Parameters.AddWithValue("@p5", "dark");
                    cmd2.ExecuteNonQuery();
                }
            }

            return RedirectToAction("AdminPanel");
        }
        [HttpGet]
        public IActionResult RandevulariAyir()
        {
            List<RandevuViewModel> gecmis = new List<RandevuViewModel>();
            List<RandevuViewModel> aktif = new List<RandevuViewModel>();
            DateTime simdi = DateTime.Now;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Randevular", conn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var model = new RandevuViewModel
                    {
                        RandevuID = Convert.ToInt32(dr["RandevuID"]),
                        Tarih = Convert.ToDateTime(dr["Tarih"]),
                        Saat = dr["Saat"].ToString(),
                        Bolum = dr["Bolum"].ToString(),
                        DoktorAd = GetDoktorAdi(Convert.ToInt32(dr["DoktorID"])),
                        HastaAd = GetHastaAdi(dr["TcKimlikNo"].ToString())
                    };

                    DateTime tarihSaat = model.Tarih.Add(TimeSpan.Parse(model.Saat));
                    if (tarihSaat < simdi)
                        gecmis.Add(model);
                    else
                        aktif.Add(model);
                }
            }

            return Json(new { gecmis, aktif });
        }

        [HttpPost]
        public IActionResult PoliklinikEkle(IFormCollection form)
        {
            string ad = form["Ad"];
            string ikon = form["Ikon"];

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Poliklinikler (Ad, Ikon) VALUES (@ad, @ikon)", conn);
                cmd.Parameters.AddWithValue("@ad", ad);
                cmd.Parameters.AddWithValue("@ikon", ikon);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("AdminPanel");
        }
        [HttpPost]
        public IActionResult PoliklinikSil(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Poliklinikler WHERE PoliklinikID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("AdminPanel");
        }
        [HttpPost]
        public IActionResult DoktorSil(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Doktorlar WHERE DoktorID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("AdminPanel");
        }
        [HttpPost]
        public IActionResult UygunlukAyarla(DoktorUygunluk uygunluk)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO DoktorUygunluk (DoktorID, BaslangicTarih, BitisTarih, BaslangicSaat, BitisSaat)
            VALUES (@DoktorID, @BaslangicTarih, @BitisTarih, @BaslangicSaat, @BitisSaat)", conn);

                cmd.Parameters.AddWithValue("@DoktorID", uygunluk.DoktorID);
                cmd.Parameters.AddWithValue("@BaslangicTarih", uygunluk.BaslangicTarih);
                cmd.Parameters.AddWithValue("@BitisTarih", uygunluk.BitisTarih);
                cmd.Parameters.AddWithValue("@BaslangicSaat", uygunluk.BaslangicSaat);
                cmd.Parameters.AddWithValue("@BitisSaat", uygunluk.BitisSaat);

                cmd.ExecuteNonQuery();
            }
            TempData["Basari"] = "✅ Müsaitlik başarıyla eklendi.";
            return RedirectToAction("AdminPanel");
        }



    }
}
