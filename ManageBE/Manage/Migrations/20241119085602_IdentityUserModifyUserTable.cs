using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manage.Migrations
{
    /// <inheritdoc />
    public partial class IdentityUserModifyUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cognome",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "AspNetUsers");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cognome",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

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
                column: "DataInserimentoCategoria",
                value: new DateTime(2024, 11, 18, 16, 55, 16, 724, DateTimeKind.Local).AddTicks(8669));

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataInserimentoCategoria",
                value: new DateTime(2024, 11, 18, 16, 55, 16, 724, DateTimeKind.Local).AddTicks(8761));
        }
    }
}
