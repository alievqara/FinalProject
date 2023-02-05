using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalProject.Migrations
{
    public partial class ProductUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Filials_FilialId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Kassa_Filials_FilialId",
                table: "Kassa");

            migrationBuilder.DropForeignKey(
                name: "FK_Musteriler_Filials_Satis_Olunan_MagazaId",
                table: "Musteriler");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Filials_FilialId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Filials");

            migrationBuilder.DropIndex(
                name: "IX_Products_FilialId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Musteriler_Satis_Olunan_MagazaId",
                table: "Musteriler");

            migrationBuilder.DropIndex(
                name: "IX_Kassa_FilialId",
                table: "Kassa");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_FilialId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FilialId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Magaza_Id",
                table: "Musteriler");

            migrationBuilder.DropColumn(
                name: "Satis_Olunan_MagazaId",
                table: "Musteriler");

            migrationBuilder.DropColumn(
                name: "FilialId",
                table: "Kassa");

            migrationBuilder.DropColumn(
                name: "FilialId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "Model_Name");

            migrationBuilder.AddColumn<string>(
                name: "Brand_Name",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brand_Name",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Model_Name",
                table: "Products",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "FilialId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Magaza_Id",
                table: "Musteriler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Satis_Olunan_MagazaId",
                table: "Musteriler",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FilialId",
                table: "Kassa",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FilialId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Filials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeletedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EditedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EditorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filials", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_FilialId",
                table: "Products",
                column: "FilialId");

            migrationBuilder.CreateIndex(
                name: "IX_Musteriler_Satis_Olunan_MagazaId",
                table: "Musteriler",
                column: "Satis_Olunan_MagazaId");

            migrationBuilder.CreateIndex(
                name: "IX_Kassa_FilialId",
                table: "Kassa",
                column: "FilialId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_FilialId",
                table: "AspNetUsers",
                column: "FilialId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Filials_FilialId",
                table: "AspNetUsers",
                column: "FilialId",
                principalTable: "Filials",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kassa_Filials_FilialId",
                table: "Kassa",
                column: "FilialId",
                principalTable: "Filials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Musteriler_Filials_Satis_Olunan_MagazaId",
                table: "Musteriler",
                column: "Satis_Olunan_MagazaId",
                principalTable: "Filials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Filials_FilialId",
                table: "Products",
                column: "FilialId",
                principalTable: "Filials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
