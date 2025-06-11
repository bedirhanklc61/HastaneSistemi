using Microsoft.EntityFrameworkCore.Migrations;

namespace HastaneSistemi.Migrations
{
    public partial class SenkronizasyonTamamlandi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DoktorUygunluklar",
                table: "DoktorUygunluklar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoktorBilgileri",
                table: "DoktorBilgileri");

            migrationBuilder.RenameTable(
                name: "DoktorUygunluklar",
                newName: "DoktorUygunluk");

            migrationBuilder.RenameTable(
                name: "DoktorBilgileri",
                newName: "Doktorlar");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoktorUygunluk",
                table: "DoktorUygunluk",
                column: "UygunlukID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Doktorlar",
                table: "Doktorlar",
                column: "DoktorID");

            migrationBuilder.CreateTable(
                name: "Adminler",
                columns: table => new
                {
                    AdminID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sifre = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adminler", x => x.AdminID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adminler");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoktorUygunluk",
                table: "DoktorUygunluk");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Doktorlar",
                table: "Doktorlar");

            migrationBuilder.RenameTable(
                name: "DoktorUygunluk",
                newName: "DoktorUygunluklar");

            migrationBuilder.RenameTable(
                name: "Doktorlar",
                newName: "DoktorBilgileri");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoktorUygunluklar",
                table: "DoktorUygunluklar",
                column: "UygunlukID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoktorBilgileri",
                table: "DoktorBilgileri",
                column: "DoktorID");
        }
    }
}
