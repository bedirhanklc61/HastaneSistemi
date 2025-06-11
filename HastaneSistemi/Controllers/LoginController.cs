using Microsoft.AspNetCore.Http;
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
                SqlCommand cmdAdmin = new SqlCommand("SELECT * FROM Adminler WHERE Email = @p1 AND Sifre = @p2", conn);
                cmdAdmin.Parameters.AddWithValue("@p1", emailOrTc);
                cmdAdmin.Parameters.AddWithValue("@p2", sifre);

                SqlDataReader drAdmin = cmdAdmin.ExecuteReader();
                if (drAdmin.Read())
                {
                    HttpContext.Session.SetString("KullaniciAdi", drAdmin["KullaniciAdi"].ToString());
                    HttpContext.Session.SetString("KullaniciTipi", "Admin");
                    drAdmin.Close();
                    return RedirectToAction("AdminPanel", "Admin");
                }
                drAdmin.Close();

                // 1️⃣ Doktor kontrolü (sadece e-posta ile giriş)
                if (emailOrTc.Contains("@"))
                {
                    SqlCommand cmdDoktor = new SqlCommand("SELECT * FROM Doktorlar WHERE Email = @p1 AND Sifre = @p2", conn);
                    cmdDoktor.Parameters.AddWithValue("@p1", emailOrTc);
                    cmdDoktor.Parameters.AddWithValue("@p2", sifre);

                    SqlDataReader drDoktor = cmdDoktor.ExecuteReader();
                    if (drDoktor.Read())
                    {
                        HttpContext.Session.SetString("Email", drDoktor["Email"].ToString());
                        HttpContext.Session.SetString("AdSoyad", drDoktor["AdSoyad"].ToString());
                        HttpContext.Session.SetString("KullaniciTipi", "Doktor");
                        HttpContext.Session.SetInt32("DoktorID", Convert.ToInt32(drDoktor["DoktorID"]));
                        drDoktor.Close();
                        return RedirectToAction("DoktorPanel", "Doktor");
                    }
                    drDoktor.Close();
                }

                // 2️⃣ Hasta kontrolü
                if (emailOrTc.Contains("@"))
                {
                    cmd = new SqlCommand("SELECT * FROM Hastalar WHERE Email = @p1 AND Sifre = @p2", conn);
                }
                else
                {
                    cmd = new SqlCommand("SELECT * FROM Hastalar WHERE TC = @p1 AND Sifre = @p2", conn);
                }

                cmd.Parameters.AddWithValue("@p1", emailOrTc);
                cmd.Parameters.AddWithValue("@p2", sifre);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    HttpContext.Session.SetString("Email", dr["Email"].ToString());
                    HttpContext.Session.SetString("AdSoyad", dr["AdSoyad"].ToString());
                    HttpContext.Session.SetString("TcKimlikNo", dr["TC"].ToString());
                    string temaModu = dr["TemaModu"]?.ToString() ?? "dark";
                    string yaziBuyuk = dr["YaziBuyuk"]?.ToString() ?? "false";

                    HttpContext.Session.SetString("TemaModu", temaModu);
                    HttpContext.Session.SetString("YaziBuyuk", yaziBuyuk);

                    dr.Close();
                    return RedirectToAction("HastaPanel", "Hasta");
                }

                dr.Close();
                ViewBag.Mesaj = "Giriş bilgileri hatalı!";
                return View("Index");
            }
        }





        [HttpPost]
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
INSERT INTO Hastalar (AdSoyad, DogumTarihi, Email, TcKimlikNo, Sifre, TemaModu, YaziBuyuk) 
VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7)", conn);

                cmd.Parameters.AddWithValue("@p1", adSoyad);
                cmd.Parameters.AddWithValue("@p2", dogumTarihi);
                cmd.Parameters.AddWithValue("@p3", email);
                cmd.Parameters.AddWithValue("@p4", tcKimlik);
                cmd.Parameters.AddWithValue("@p5", sifre);
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
    }
}
