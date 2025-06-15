using Microsoft.EntityFrameworkCore.Migrations;

namespace HastaneSistemi.Migrations
{
    public partial class PoliklinikSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Poliklinikler",
                columns: new[] { "PoliklinikID", "Ad", "Ikon" },
                values: new object[,]
                {
                    { 1, "Acil Tıp", "uil-ambulance" },
                    { 28, "Romatoloji", "uil-wheelchair" },
                    { 29, "Ruh Sağlığı ve Hastalıkları", "uil-user" },
                    { 30, "Tibbi Biyokimya", "uil-flask" },
                    { 31, "Tibbi Genetik", "uil-dna" },
                    { 32, "Tibbi Mikrobiyoloji", "uil-virus-slash" },
                    { 33, "Tibbi Onkoloji", "uil-dna" },
                    { 34, "Tibbi Patoloji", "uil-notes" },
                    { 35, "Yeni Doğan", "uil-baby-carriage" },
                    { 36, "Çocuk Acil", "uil-ambulance" },
                    { 37, "Çocuk Cerrahisi", "uil-syringe" },
                    { 38, "Çocuk Endokrinolojisi", "uil-baby-carriage" },
                    { 39, "Çocuk Enfeksiyon ve Hastalıkları", "uil-virus-slash" },
                    { 40, "Çocuk Gastroenterolojisi", "uil-stethoscope-alt" },
                    { 41, "Çocuk Hematolojisi ve Onkolojisi", "uil-dna" },
                    { 42, "Çocuk Kardiyolojisi", "uil-heart" },
                    { 43, "Çocuk Nefrolojisi", "uil-kid" },
                    { 44, "Çocuk Nörolojisi", "uil-brain" },
                    { 45, "Çocuk Romatolojisi", "uil-wheelchair-alt" },
                    { 46, "Çocuk Sağlığı ve Hastalıkları", "uil-kid" },
                    { 47, "Çocuk ve Ergen Ruh Sağlığı", "uil-user" },
                    { 48, "Çocuk İmmünolojisi ve Alerji Hastalıkları", "uil-syringe" },
                    { 27, "Radyoloji", "uil-dna" },
                    { 26, "Radyasyon Onkolojisi", "uil-atom" },
                    { 25, "Plastik, Rekonstrüktif ve Estetik Cerrahi", "uil-user-check" },
                    { 24, "Ortopedi", "uil-wheelchair" },
                    { 2, "Aile Hekimliği", "uil-user-md" },
                    { 3, "Anesteziyoloji ve Reanimasyon", "uil-clinic-medical" },
                    { 4, "Beyin ve Sinir Cerrahisi", "uil-brain" },
                    { 5, "Cildiye", "uil-swatchbook" },
                    { 6, "Endokrinoloji ve Metabolizma Hastalıkları", "uil-flask" },
                    { 7, "Enfeksiyon Hastalıkları", "uil-virus-slash" },
                    { 8, "Fiziksel Tıp ve Rehabilitasyon", "uil-wheelchair" },
                    { 9, "Gastroenteroloji", "uil-stethoscope" },
                    { 10, "Genel Cerrahi", "uil-hospital" },
                    { 11, "Genel Dahiliye", "uil-medical-drip" },
                    { 49, "Üroloji", "uil-medkit" },
                    { 12, "Girişimsel Radyoloji", "uil-flask" },
                    { 14, "Göğüs Cerrahisi", "uil-stethoscope" },
                    { 15, "Göğüs Hastalıkları", "uil-stethoscope" },
                    { 16, "Hematoloji", "uil-syringe" },
                    { 17, "Kadin Hastalıkları", "uil-user" }
                });

            migrationBuilder.InsertData(
                table: "Poliklinikler",
                columns: new[] { "PoliklinikID", "Ad", "Ikon" },
                values: new object[,]
                {
                    { 18, "Kalp ve Damar Cerrahisi", "uil-heart" },
                    { 19, "Kardiyoloji", "uil-heartbeat" },
                    { 20, "Kulak Burun Boğaz", "uil-headphones" },
                    { 21, "Nefroloji", "uil-kid" },
                    { 22, "Nöroloji", "uil-brain" },
                    { 23, "Nükleer Tıp", "uil-atom" },
                    { 13, "Göz Hastalıkları", "uil-eye" },
                    { 50, "İmmünoloji ve Alerji Hastalıkları", "uil-syringe" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Poliklinikler",
                keyColumn: "PoliklinikID",
                keyValue: 50);
        }
    }
}
