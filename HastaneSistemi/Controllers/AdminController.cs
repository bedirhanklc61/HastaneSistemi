using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HastaneSistemi.Models;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;


namespace HastaneSistemi.Controllers
{
    public class AdminController : Controller
    {
        private readonly string _connectionString = "Data Source=127.0.0.1,1433;Initial Catalog=HastaneDB;User ID=sa;Password=12345;TrustServerCertificate=True;";

        public IActionResult AdminPanel()
        {
            List<Poliklinik> poliklinikler = new List<Poliklinik>();
            List<RandevuViewModel> randevular = new List<RandevuViewModel>();

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
            }

            ViewBag.Poliklinikler = poliklinikler;
            ViewBag.Randevular = randevular;

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
                SqlCommand cmd = new SqlCommand("SELECT AdSoyad FROM Hastalar WHERE TcKimlikNo = @tc", conn);
                cmd.Parameters.AddWithValue("@tc", tcKimlik);
                return cmd.ExecuteScalar()?.ToString() ?? "Bilinmiyor";
            }
        }



    }
}
