using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Manage.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Utenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cognome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInserimentoUtente = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utenti", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CategoriaDocumenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomeCategoria = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DescrizioneCategoria = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInserimentoCategoria = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsPredefinita = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UtenteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaDocumenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoriaDocumenti_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Documenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Titolo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descrizione = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataCreazioneDocumento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataInserimentoDocumento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UtenteId = table.Column<int>(type: "int", nullable: false),
                    CategoriaDocumentiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documenti_CategoriaDocumenti_CategoriaDocumentiId",
                        column: x => x.CategoriaDocumentiId,
                        principalTable: "CategoriaDocumenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documenti_Utenti_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "Utenti",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FileDocumenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NomeFile = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EstensioneFile = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PercorsoFile = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInserimentoFile = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DocumentiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileDocumenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileDocumenti_Documenti_DocumentiId",
                        column: x => x.DocumentiId,
                        principalTable: "Documenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "CategoriaDocumenti",
                columns: new[] { "Id", "DataInserimentoCategoria", "DescrizioneCategoria", "IsPredefinita", "NomeCategoria", "UtenteId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 9, 23, 15, 25, 14, 402, DateTimeKind.Local).AddTicks(2296), "Descrizione categoria Predefinita 1", true, "Nome categoria Predefinita 1", null },
                    { 2, new DateTime(2024, 9, 23, 15, 25, 14, 402, DateTimeKind.Local).AddTicks(2401), "Descrizione categoria Predefinita 2", true, "Nome categoria Predefinita 2", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaDocumenti_UtenteId",
                table: "CategoriaDocumenti",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Documenti_CategoriaDocumentiId",
                table: "Documenti",
                column: "CategoriaDocumentiId");

            migrationBuilder.CreateIndex(
                name: "IX_Documenti_UtenteId",
                table: "Documenti",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_FileDocumenti_DocumentiId",
                table: "FileDocumenti",
                column: "DocumentiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileDocumenti");

            migrationBuilder.DropTable(
                name: "Documenti");

            migrationBuilder.DropTable(
                name: "CategoriaDocumenti");

            migrationBuilder.DropTable(
                name: "Utenti");
        }
    }
}
