using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manage.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigrationNetWorth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Investimenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ticker = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Isin = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrezzoAttualeInvestimento = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PrezzoMedio = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PrezzoMinimo = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    PrezzoMassimo = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TipoInvestimento = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UtenteId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoInvestimentoShadow = table.Column<string>(type: "varchar(34)", maxLength: 34, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Settore = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsAccumulo = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    DividendYield = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    PenalitaPrelievoAnticipatoAttiva = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    PenalitaPercentuale = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    HasPenalita = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    ContoDeposito_PenalitaPercentuale = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    AliquotaTasse = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ImpostaBolloAnnuale = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CostoGestioneFisso = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Blockchain = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TassoStaking = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    PercentualeStipendio = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    PercentualeStipendioDatoreLavoro = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    InteresseAnnuale = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    FondoPensione_PenalitaPrelievoAnticipatoAttiva = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    PenalitaPrelievoPercentuale = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    PrezzoAcquisto = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ValoreAttuale = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    RedditoAnnualeAffitto = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CostiGestioneAnnui = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    AliquotaFiscale = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    DataAcquisto = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CedolaAnnua = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    DataScadenza = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    HasPenalitaAnticipata = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    PenalitaAnticipataPercentuale = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    BonusMantenimento = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    PenalitaAnticipata = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ValoreRimborso = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TitoloDiStato_PenalitaPercentuale = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TitoloDiStato_HasPenalita = table.Column<bool>(type: "tinyint(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investimenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investimenti_AspNetUsers_UtenteId",
                        column: x => x.UtenteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CedoleTitoloDiStato",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Anno = table.Column<int>(type: "int", nullable: false),
                    PercentualeCedola = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TitoloDiStatoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CedoleTitoloDiStato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CedoleTitoloDiStato_Investimenti_TitoloDiStatoId",
                        column: x => x.TitoloDiStatoId,
                        principalTable: "Investimenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RendimentiBuoniFruttiferi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Durata = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    PercentualeRendimento = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    BuoniFruttiferiPostaliId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RendimentiBuoniFruttiferi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RendimentiBuoniFruttiferi_Investimenti_BuoniFruttiferiPostal~",
                        column: x => x.BuoniFruttiferiPostaliId,
                        principalTable: "Investimenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TassiContoDeposito",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Durata = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    TassoInteresse = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ContoDepositoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TassiContoDeposito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TassiContoDeposito_Investimenti_ContoDepositoId",
                        column: x => x.ContoDepositoId,
                        principalTable: "Investimenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Transazioni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvestimentoId = table.Column<int>(type: "int", nullable: false),
                    DataTransazione = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PrezzoUnitario = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Quantita = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Commissione = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TipoTransazione = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transazioni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transazioni_Investimenti_InvestimentoId",
                        column: x => x.InvestimentoId,
                        principalTable: "Investimenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataInserimentoCategoria",
                value: new DateTime(2024, 11, 26, 9, 29, 6, 473, DateTimeKind.Local).AddTicks(3083));

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataInserimentoCategoria",
                value: new DateTime(2024, 11, 26, 9, 29, 6, 473, DateTimeKind.Local).AddTicks(3182));

            migrationBuilder.CreateIndex(
                name: "IX_CedoleTitoloDiStato_TitoloDiStatoId",
                table: "CedoleTitoloDiStato",
                column: "TitoloDiStatoId");

            migrationBuilder.CreateIndex(
                name: "IX_Investimenti_UtenteId",
                table: "Investimenti",
                column: "UtenteId");

            migrationBuilder.CreateIndex(
                name: "IX_RendimentiBuoniFruttiferi_BuoniFruttiferiPostaliId",
                table: "RendimentiBuoniFruttiferi",
                column: "BuoniFruttiferiPostaliId");

            migrationBuilder.CreateIndex(
                name: "IX_TassiContoDeposito_ContoDepositoId",
                table: "TassiContoDeposito",
                column: "ContoDepositoId");

            migrationBuilder.CreateIndex(
                name: "IX_Transazioni_InvestimentoId",
                table: "Transazioni",
                column: "InvestimentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CedoleTitoloDiStato");

            migrationBuilder.DropTable(
                name: "RendimentiBuoniFruttiferi");

            migrationBuilder.DropTable(
                name: "TassiContoDeposito");

            migrationBuilder.DropTable(
                name: "Transazioni");

            migrationBuilder.DropTable(
                name: "Investimenti");

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataInserimentoCategoria",
                value: new DateTime(2024, 11, 19, 9, 56, 2, 352, DateTimeKind.Local).AddTicks(1636));

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataInserimentoCategoria",
                value: new DateTime(2024, 11, 19, 9, 56, 2, 352, DateTimeKind.Local).AddTicks(1731));
        }
    }
}
