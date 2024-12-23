using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Manage.Migrations
{
    /// <inheritdoc />
    public partial class InitialModifyCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SottoCategoriaDocumentiId",
                table: "Documenti",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SottoCategoriaDocumenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomeSottoCategoria = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DescrizioneSottoCategoria = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInserimentoSottoCategoria = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CategoriaDocumentiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SottoCategoriaDocumenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SottoCategoriaDocumenti_CategoriaDocumenti_CategoriaDocument~",
                        column: x => x.CategoriaDocumentiId,
                        principalTable: "CategoriaDocumenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataInserimentoCategoria", "DescrizioneCategoria", "NomeCategoria" },
                values: new object[] { new DateTime(2024, 11, 29, 13, 34, 5, 793, DateTimeKind.Local).AddTicks(3366), "Documenti relativi alle bollette", "Bollette" });

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DataInserimentoCategoria", "DescrizioneCategoria", "NomeCategoria" },
                values: new object[] { new DateTime(2024, 11, 29, 13, 34, 5, 793, DateTimeKind.Local).AddTicks(3458), "Documenti relativi alle fatture", "Fatture" });

            migrationBuilder.InsertData(
                table: "SottoCategoriaDocumenti",
                columns: new[] { "Id", "CategoriaDocumentiId", "DataInserimentoSottoCategoria", "DescrizioneSottoCategoria", "NomeSottoCategoria" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 11, 29, 13, 34, 5, 793, DateTimeKind.Local).AddTicks(3746), "Bollette internet", "Bollette Internet" },
                    { 2, 1, new DateTime(2024, 11, 29, 13, 34, 5, 793, DateTimeKind.Local).AddTicks(3837), "Bollette energia elettrica", "Bollette Energia" },
                    { 3, 2, new DateTime(2024, 11, 29, 13, 34, 5, 793, DateTimeKind.Local).AddTicks(3927), "Fatture telefoniche", "Fatture Internet" },
                    { 4, 2, new DateTime(2024, 11, 29, 13, 34, 5, 793, DateTimeKind.Local).AddTicks(4016), "Fatture energia elettrica", "Fatture Energia" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documenti_SottoCategoriaDocumentiId",
                table: "Documenti",
                column: "SottoCategoriaDocumentiId");

            migrationBuilder.CreateIndex(
                name: "IX_SottoCategoriaDocumenti_CategoriaDocumentiId",
                table: "SottoCategoriaDocumenti",
                column: "CategoriaDocumentiId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documenti_SottoCategoriaDocumenti_SottoCategoriaDocumentiId",
                table: "Documenti",
                column: "SottoCategoriaDocumentiId",
                principalTable: "SottoCategoriaDocumenti",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documenti_SottoCategoriaDocumenti_SottoCategoriaDocumentiId",
                table: "Documenti");

            migrationBuilder.DropTable(
                name: "SottoCategoriaDocumenti");

            migrationBuilder.DropIndex(
                name: "IX_Documenti_SottoCategoriaDocumentiId",
                table: "Documenti");

            migrationBuilder.DropColumn(
                name: "SottoCategoriaDocumentiId",
                table: "Documenti");

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataInserimentoCategoria", "DescrizioneCategoria", "NomeCategoria" },
                values: new object[] { new DateTime(2024, 11, 26, 9, 29, 6, 473, DateTimeKind.Local).AddTicks(3083), "Descrizione categoria Predefinita 1", "Nome categoria Predefinita 1" });

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DataInserimentoCategoria", "DescrizioneCategoria", "NomeCategoria" },
                values: new object[] { new DateTime(2024, 11, 26, 9, 29, 6, 473, DateTimeKind.Local).AddTicks(3182), "Descrizione categoria Predefinita 2", "Nome categoria Predefinita 2" });
        }
    }
}
