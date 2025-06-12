using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using HastaneSistemi.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.Extensions.Configuration;



namespace HastaneSistemi.Controllers
{
    public class HastaController : Controller
    {
        private readonly string _connectionString;
        private readonly HastaneDbContext _context;

        public HastaController(HastaneDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("HastaneDB");
        }


        public IActionResult HastaPanel()
        {
            string email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index", "Login");
            }

            RandevuViewModel randevu = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // TC'yi al
                SqlCommand cmdTc = new SqlCommand("SELECT TC, YaziBuyuk FROM Hastalar WHERE Email = @p1", conn);
                cmdTc.Parameters.AddWithValue("@p1", email);
                SqlDataReader drTc = cmdTc.ExecuteReader();

                string tc = null;
                bool yaziBuyuk = false;

                if (drTc.Read())
                {
                    tc = drTc["TC"].ToString();
                    yaziBuyuk = Convert.ToBoolean(drTc["YaziBuyuk"]);
                }
                drTc.Close();

                // Yazı büyüklüğü bilgisini session'a ata
                HttpContext.Session.SetString("YaziBuyuk", yaziBuyuk.ToString());

                if (!string.IsNullOrEmpty(tc))
                {
                    SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM Randevular WHERE TcKimlikNo = @p1 ORDER BY Tarih, Saat", conn);
                    cmd.Parameters.AddWithValue("@p1", tc);
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        randevu = new RandevuViewModel
                        {
                            RandevuID = Convert.ToInt32(dr["RandevuID"]),
                            Tarih = Convert.ToDateTime(dr["Tarih"]),
                            Saat = dr["Saat"].ToString(),
                            Bolum = dr["Bolum"].ToString(),
                            DoktorAd = GetDoktorAdi(Convert.ToInt32(dr["DoktorID"]))
                        };
                    }
                    dr.Close();
                }
            }

            ViewBag.Randevu = randevu;
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




        public IActionResult RandevuAl()
        {
            return View();
        }

        public JsonResult DoktorlariGetir()
        {
            List<Doktor> doktorlar = new List<Doktor>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT DoktorID, AdSoyad, Bolum FROM Doktorlar", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    doktorlar.Add(new Doktor
                    {
                        DoktorID = Convert.ToInt32(reader["DoktorID"]),
                        AdSoyad = reader["AdSoyad"].ToString(),
                        Bolum = reader["Bolum"].ToString()
                    });
                }
                reader.Close();
            }
            return Json(doktorlar);
        }




        public IActionResult Ayarlar(string returnUrl)
        {
            string email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index", "Login");
            }

            HastaBilgileri model = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT AdSoyad, TC, Email, DogumTarihi, Sifre, TemaModu, YaziBuyuk FROM Hastalar WHERE Email = @p1", conn);
                cmd.Parameters.AddWithValue("@p1", email);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    model = new HastaBilgileri
                    {
                        AdSoyad = dr["AdSoyad"].ToString(),
                        TC = dr["TC"].ToString(),
                        Email = dr["Email"].ToString(),
                        DogumTarihi = Convert.ToDateTime(dr["DogumTarihi"]),
                        Sifre = string.Empty,
                        TemaModu = dr["TemaModu"].ToString(),
                        YaziBuyuk = Convert.ToBoolean(dr["YaziBuyuk"])
                    };
                    HttpContext.Session.SetString("YaziBuyuk", model.YaziBuyuk ? "true" : "false");
                }
                dr.Close();
            }

            ViewBag.ReturnUrl = returnUrl ?? "/Hasta/HastaPanel"; 
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AyarlarGuncelle(HastaBilgileri model)
        {
            string email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Index", "Login");
            }

            model.TemaModu = Request.Form["TemaModu"];
            string yaziBuyukDegeri = Request.Form["YaziBuyuk"];
            bool yaziBuyuk = yaziBuyukDegeri == "true";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd;
                if (!string.IsNullOrEmpty(model.Sifre))
                {
                    cmd = new SqlCommand(@"
            UPDATE Hastalar
            SET Email = @p1, DogumTarihi = @p2, Sifre = @p3, TemaModu = @p4, YaziBuyuk = @p5
            WHERE Email = @p6", conn);
                    var hasher = new PasswordHasher<HastaBilgileri>();
                    string hashed = hasher.HashPassword(null, model.Sifre);
                    cmd.Parameters.AddWithValue("@p3", hashed);
                }
                else
                {
                    cmd = new SqlCommand(@"
            UPDATE Hastalar
            SET Email = @p1, DogumTarihi = @p2, TemaModu = @p4, YaziBuyuk = @p5
            WHERE Email = @p6", conn);
                }

                cmd.Parameters.AddWithValue("@p1", model.Email);
                cmd.Parameters.AddWithValue("@p2", model.DogumTarihi);
                cmd.Parameters.AddWithValue("@p4", model.TemaModu);
                cmd.Parameters.AddWithValue("@p5", yaziBuyuk);
                cmd.Parameters.AddWithValue("@p6", email);

                cmd.ExecuteNonQuery();
            }

            HttpContext.Session.SetString("TemaModu", model.TemaModu);
            HttpContext.Session.SetString("YaziBuyuk", yaziBuyuk.ToString());

            return RedirectToAction("Ayarlar");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TemaDegistir([FromBody] JsonElement data)
        {
            string yeniTema = data.GetProperty("tema").GetString();
            string email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email))
            {
                return Unauthorized();
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE Hastalar SET TemaModu = @p1 WHERE Email = @p2", conn);
                cmd.Parameters.AddWithValue("@p1", yeniTema);
                cmd.Parameters.AddWithValue("@p2", email);
                cmd.ExecuteNonQuery();
            }

            HttpContext.Session.SetString("TemaModu", yeniTema);

            return Ok();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RandevuOlustur([FromBody] RandevuModel model)
        {
            // Daha önceden o saatte randevu var mı?
            bool randevuVar = false;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmdKontrol = new SqlCommand(@"
    SELECT COUNT(*) FROM Randevular
    WHERE DoktorID = @doktorID AND Tarih = @tarih AND Saat = @saat", conn);
                cmdKontrol.Parameters.AddWithValue("@doktorID", model.DoktorID);
                cmdKontrol.Parameters.AddWithValue("@tarih", model.Tarih);
                cmdKontrol.Parameters.AddWithValue("@saat", model.Saat);

                int count = (int)cmdKontrol.ExecuteScalar();
                randevuVar = count > 0;

                if (randevuVar)
                    return BadRequest("Bu saat dolu.");


                // Randevuyu kaydet
                var cmdEkle = new SqlCommand(@"
    INSERT INTO Randevular (TcKimlikNo, Bolum, DoktorID, Tarih, Saat)
    VALUES (@tc, @bolum, @doktorID, @tarih, @saat)", conn);
                cmdEkle.Parameters.AddWithValue("@tc", HttpContext.Session.GetString("TC"));
                cmdEkle.Parameters.AddWithValue("@bolum", model.Bolum);
                cmdEkle.Parameters.AddWithValue("@doktorID", model.DoktorID);
                cmdEkle.Parameters.AddWithValue("@tarih", model.Tarih);
                cmdEkle.Parameters.AddWithValue("@saat", model.Saat);
                cmdEkle.ExecuteNonQuery();
            }

            return Ok();
        }



        public JsonResult RandevulariGetir()    
        {
            string email = HttpContext.Session.GetString("Email");
            List<RandevuViewModel> randevular = new List<RandevuViewModel>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Email'den hastanın TcKimlikNo'sunu al
                SqlCommand cmdTc = new SqlCommand("SELECT TC FROM Hastalar WHERE Email = @p1", conn);
                cmdTc.Parameters.AddWithValue("@p1", email);
                string tc = cmdTc.ExecuteScalar()?.ToString();

                if (!string.IsNullOrEmpty(tc))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Randevular WHERE TcKimlikNo = @tc", conn);
                    cmd.Parameters.AddWithValue("@tc", tc);

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        int doktorID = Convert.ToInt32(dr["DoktorID"]);
                        string doktorAd = GetDoktorAdi(doktorID);

                        randevular.Add(new RandevuViewModel
                        {
                            RandevuID = Convert.ToInt32(dr["RandevuID"]),
                            Tarih = Convert.ToDateTime(dr["Tarih"]),
                            Saat = dr["Saat"].ToString(),
                            Bolum = dr["Bolum"].ToString(),
                            DoktorAd = doktorAd
                        });
                    }
                    dr.Close();
                }
            }

            return Json(randevular);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RandevuIptalEt([FromBody] JsonElement data)
        {
            if (!data.TryGetProperty("randevuID", out JsonElement idElement))
                return BadRequest("Geçersiz veri");

            int randevuID;
            if (idElement.ValueKind == JsonValueKind.Number)
            {
                randevuID = idElement.GetInt32();
            }
            else if (idElement.ValueKind == JsonValueKind.String &&
                     int.TryParse(idElement.GetString(), out int parsed))
            {
                randevuID = parsed;
            }
            else
            {
                return BadRequest("ID dönüştürülemedi");
            }

            string tc = HttpContext.Session.GetString("TC");
            if (string.IsNullOrEmpty(tc))
                return Unauthorized();


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                var kontrolCmd = new SqlCommand("SELECT COUNT(*) FROM Randevular WHERE RandevuID = @id AND TcKimlikNo = @tc", conn);
                kontrolCmd.Parameters.AddWithValue("@id", randevuID);
                kontrolCmd.Parameters.AddWithValue("@tc", tc);
                int sahipMi = (int)kontrolCmd.ExecuteScalar();

                if (sahipMi == 0)
                    return Unauthorized();

                SqlCommand cmd = new SqlCommand("DELETE FROM Randevular WHERE RandevuID = @id", conn);
                cmd.Parameters.AddWithValue("@id", randevuID);
                int sonuc = cmd.ExecuteNonQuery();

                if (sonuc > 0)
                    return Ok();
                else
                    return BadRequest("Silme başarısız");
            }
        }

        [HttpGet]
        public JsonResult GetDoktorUygunluk(int doktorID)
        {
            DoktorUygunluk uy = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
            SELECT TOP 1 * FROM DoktorUygunluk
            WHERE DoktorID = @d ORDER BY UygunlukID DESC", conn);
                cmd.Parameters.AddWithValue("@d", doktorID);
                using (var dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        uy = new DoktorUygunluk
                        {
                            BaslangicTarih = Convert.ToDateTime(dr["BaslangicTarih"]),
                            BitisTarih = Convert.ToDateTime(dr["BitisTarih"]),
                            BaslangicSaat = (TimeSpan)dr["BaslangicSaat"],
                            BitisSaat = (TimeSpan)dr["BitisSaat"]
                        };
                    }
                }
            }

            if (uy == null)
                return Json(null);

            return Json(new
            {
                baslangicTarih = uy.BaslangicTarih.ToString("yyyy-MM-dd"),
                bitisTarih = uy.BitisTarih.ToString("yyyy-MM-dd"),
                baslangicSaat = uy.BaslangicSaat.ToString(@"hh\:mm"),
                bitisSaat = uy.BitisSaat.ToString(@"hh\:mm")
            });
        }

        [HttpGet]
        public JsonResult DoluSaatleriGetir(int doktorID, string tarih)
        {
            List<string> doluSaatler = new List<string>();

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"
            SELECT Saat FROM Randevular
            WHERE DoktorID = @doktorID AND Tarih = @tarih", conn);
                cmd.Parameters.AddWithValue("@doktorID", doktorID);
                cmd.Parameters.AddWithValue("@tarih", tarih); // "yyyy-MM-dd"

                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    doluSaatler.Add(dr["Saat"].ToString());

                }
            }

            return Json(doluSaatler); // ["10:15", "11:00", ...]
        }

        public IActionResult PoliklinikleriGetir()
        {
            var poliklinikler = _context.Poliklinikler
                .Select(p => new {
                    ad = p.Ad,
                    ikon = p.Ikon
                }).ToList();

            return Json(poliklinikler);
        }


    }
}
