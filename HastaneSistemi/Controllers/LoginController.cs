using HastaneSistemi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace HastaneSistemi.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("HastaneDB");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GirisYap(string emailOrTc, string sifre)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd;

                if (string.IsNullOrEmpty(emailOrTc))
                {
                    ViewBag.Mesaj = "Lütfen Email veya TC Kimlik Numarası giriniz.";
                    return View("Index");
                }

                // 0️⃣ Admin kontrolü
                SqlCommand cmdAdmin = new SqlCommand("SELECT * FROM Adminler WHERE Email = @p1", conn);
                cmdAdmin.Parameters.AddWithValue("@p1", emailOrTc);
               

                SqlDataReader drAdmin = cmdAdmin.ExecuteReader();
                if (drAdmin.Read())
                {
                    string hashed = drAdmin["Sifre"].ToString();
                    var hasher = new PasswordHasher<AdminBilgileri>();
                    bool valid = false;
                    try
                    {
                        valid = hasher.VerifyHashedPassword(null, hashed, sifre) == PasswordVerificationResult.Success;
                    }
                    catch (FormatException)
                    {
                        valid = hashed == sifre;
                    }
                    if (valid)
                    {
                        HttpContext.Session.SetString("Email", drAdmin["Email"].ToString());
                        HttpContext.Session.SetString("KullaniciTipi", "Admin");
                        drAdmin.Close();
                        return RedirectToAction("AdminPanel", "Admin");
                    }
                }
                drAdmin.Close();

                // 1️⃣ Doktor kontrolü (sadece e-posta ile giriş)
                if (emailOrTc.Contains("@"))
                {
                    SqlCommand cmdDoktor = new SqlCommand("SELECT * FROM Doktorlar WHERE Email = @p1", conn);
                    cmdDoktor.Parameters.AddWithValue("@p1", emailOrTc);

                    SqlDataReader drDoktor = cmdDoktor.ExecuteReader();
                    if (drDoktor.Read())
                    {
                        string hashed = drDoktor["Sifre"].ToString();
                        var hasher = new PasswordHasher<DoktorBilgileri>();
                        bool valid = false;
                        try
                        {
                            valid = hasher.VerifyHashedPassword(null, hashed, sifre) == PasswordVerificationResult.Success;
                        }
                        catch (FormatException)
                        {
                            valid = hashed == sifre;
                        }
                        if (valid)
                        {
                            HttpContext.Session.SetString("Email", drDoktor["Email"].ToString());
                            HttpContext.Session.SetString("AdSoyad", drDoktor["AdSoyad"].ToString());
                            HttpContext.Session.SetString("KullaniciTipi", "Doktor");
                            HttpContext.Session.SetInt32("DoktorID", Convert.ToInt32(drDoktor["DoktorID"]));
                            drDoktor.Close();
                            return RedirectToAction("DoktorPanel", "Doktor");
                        }
                    }
                    drDoktor.Close();
                }

                // 2️⃣ Hasta kontrolü
                if (emailOrTc.Contains("@"))
                {
                    cmd = new SqlCommand("SELECT * FROM Hastalar WHERE Email = @p1", conn);
                }
                else
                {
                    cmd = new SqlCommand("SELECT * FROM Hastalar WHERE TC = @p1", conn);
                }

                cmd.Parameters.AddWithValue("@p1", emailOrTc);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    string hashed = dr["Sifre"].ToString();
                    var hasher = new PasswordHasher<HastaBilgileri>();
                    bool valid = false;
                    try
                    {
                        valid = hasher.VerifyHashedPassword(null, hashed, sifre) == PasswordVerificationResult.Success;
                    }
                    catch (FormatException)
                    {
                        valid = hashed == sifre;
                    }
                    if (valid)
                    {
                        HttpContext.Session.SetString("Email", dr["Email"].ToString());
                        HttpContext.Session.SetString("AdSoyad", dr["AdSoyad"].ToString());
                        HttpContext.Session.SetString("TC", dr["TC"].ToString());
                        string temaModu = dr["TemaModu"]?.ToString() ?? "dark";
                        string yaziBuyuk = dr["YaziBuyuk"]?.ToString() ?? "false";

                        HttpContext.Session.SetString("TemaModu", temaModu);
                        HttpContext.Session.SetString("YaziBuyuk", yaziBuyuk);

                        dr.Close();
                        return RedirectToAction("HastaPanel", "Hasta");
                    }
                }

                dr.Close();
                ViewBag.Mesaj = "Giriş bilgileri hatalı!";
                return View("Index");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KayitOl(IFormCollection form)
        {
            string adSoyad = form["adsoyad"];
            string email = form["email"];
            string tcKimlik = form["tc"];
            string sifre = form["sifre"];

            // Doğum tarihi hem DateTime olarak işleniyor hem veritabanına yazılacak
            DateTime dogumTarihi = DateTime.Parse(form["dogumTarihi"]);
            bool yaziBuyuk = (DateTime.Now.Year - dogumTarihi.Year) >= 65;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(@"
INSERT INTO Hastalar (AdSoyad, DogumTarihi, Email, TC, Sifre, TemaModu, YaziBuyuk) 
VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7)", conn);

                var hasher = new PasswordHasher<HastaBilgileri>();
                string hashed = hasher.HashPassword(null, sifre);
                cmd.Parameters.AddWithValue("@p1", adSoyad);
                cmd.Parameters.AddWithValue("@p2", dogumTarihi);
                cmd.Parameters.AddWithValue("@p3", email);
                cmd.Parameters.AddWithValue("@p4", tcKimlik);
                cmd.Parameters.AddWithValue("@p5", hashed);
                cmd.Parameters.AddWithValue("@p6", "dark");
                cmd.Parameters.AddWithValue("@p7", yaziBuyuk);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index", "Login");
        }

        
        public IActionResult CikisYap()
        {
            HttpContext.Session.Clear(); // Tüm oturum bilgilerini temizle
            return RedirectToAction("Index", "Login");
        }

        public IActionResult HashUret()
        {
            var hasher = new PasswordHasher<string>();
            string hashed = hasher.HashPassword(null, "1234");
            return Content("Hashli Şifre: " + hashed);
        }

    }
}
