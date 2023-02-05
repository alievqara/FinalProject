using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    public partial class Musteri : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MusteriId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Musteriler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeTel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZaminTel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Yeri = table.Column<bool>(type: "bit", nullable: false),
                    Is_Yerinin_Adi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Is_Staji = table.Column<double>(type: "float", nullable: false),
                    UmumiDeyer_Borc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Kredit_Nagd = table.Column<bool>(type: "bit", nullable: false),
                    Kredit_Muddeti = table.Column<int>(type: "int", nullable: false),
                    Kredit_Faizi = table.Column<int>(type: "int", nullable: false),
                    OdemeTarixi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IlkinOdenis = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaticiId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Satici_Id = table.Column<int>(type: "int", nullable: false),
                    Satis_Olunan_MagazaId = table.Column<int>(type: "int", nullable: false),
                    Magaza_Id = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EditedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Musteriler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Musteriler_AspNetUsers_SaticiId",
                        column: x => x.SaticiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Musteriler_Filials_Satis_Olunan_MagazaId",
                        column: x => x.Satis_Olunan_MagazaId,
                        principalTable: "Filials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_MusteriId",
                table: "Products",
                column: "MusteriId");

            migrationBuilder.CreateIndex(
                name: "IX_Musteriler_SaticiId",
                table: "Musteriler",
                column: "SaticiId");

            migrationBuilder.CreateIndex(
                name: "IX_Musteriler_Satis_Olunan_MagazaId",
                table: "Musteriler",
                column: "Satis_Olunan_MagazaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Musteriler_MusteriId",
                table: "Products",
                column: "MusteriId",
                principalTable: "Musteriler",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Musteriler_MusteriId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Musteriler");

            migrationBuilder.DropIndex(
                name: "IX_Products_MusteriId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MusteriId",
                table: "Products");
        }
    }
}
