using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manage.Migrations
{
    /// <inheritdoc />
    public partial class IdentityUserModify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriaDocumenti_Utenti_UtenteId",
                table: "CategoriaDocumenti");

            migrationBuilder.DropForeignKey(
                name: "FK_Documenti_Utenti_UtenteId",
                table: "Documenti");

            migrationBuilder.DropTable(
                name: "Utenti");

            migrationBuilder.AlterColumn<string>(
                name: "UtenteId",
                table: "Documenti",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "UtenteId",
                table: "CategoriaDocumenti",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Cognome",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataInserimentoUtente",
                table: "AspNetUsers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataInserimentoCategoria", "UtenteId" },
                values: new object[] { new DateTime(2024, 11, 18, 16, 55, 16, 724, DateTimeKind.Local).AddTicks(8669), null });

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DataInserimentoCategoria", "UtenteId" },
                values: new object[] { new DateTime(2024, 11, 18, 16, 55, 16, 724, DateTimeKind.Local).AddTicks(8761), null });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriaDocumenti_AspNetUsers_UtenteId",
                table: "CategoriaDocumenti",
                column: "UtenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documenti_AspNetUsers_UtenteId",
                table: "Documenti",
                column: "UtenteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriaDocumenti_AspNetUsers_UtenteId",
                table: "CategoriaDocumenti");

            migrationBuilder.DropForeignKey(
                name: "FK_Documenti_AspNetUsers_UtenteId",
                table: "Documenti");

            migrationBuilder.DropColumn(
                name: "Cognome",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DataInserimentoUtente",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "UtenteId",
                table: "Documenti",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "UtenteId",
                table: "CategoriaDocumenti",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Utenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cognome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInserimentoUtente = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utenti", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DataInserimentoCategoria", "UtenteId" },
                values: new object[] { new DateTime(2024, 11, 18, 9, 49, 5, 759, DateTimeKind.Local).AddTicks(665), null });

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DataInserimentoCategoria", "UtenteId" },
                values: new object[] { new DateTime(2024, 11, 18, 9, 49, 5, 759, DateTimeKind.Local).AddTicks(757), null });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriaDocumenti_Utenti_UtenteId",
                table: "CategoriaDocumenti",
                column: "UtenteId",
                principalTable: "Utenti",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documenti_Utenti_UtenteId",
                table: "Documenti",
                column: "UtenteId",
                principalTable: "Utenti",
                principalColumn: "Id");
        }
    }
}
