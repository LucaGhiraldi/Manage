using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manage.Migrations
{
    /// <inheritdoc />
    public partial class InitialModifyCategoryDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documenti_SottoCategoriaDocumenti_SottoCategoriaDocumentiId",
                table: "Documenti");

            migrationBuilder.DropForeignKey(
                name: "FK_SottoCategoriaDocumenti_CategoriaDocumenti_CategoriaDocument~",
                table: "SottoCategoriaDocumenti");

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataInserimentoCategoria",
                value: new DateTime(2024, 12, 2, 17, 21, 51, 173, DateTimeKind.Local).AddTicks(3912));

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataInserimentoCategoria",
                value: new DateTime(2024, 12, 2, 17, 21, 51, 173, DateTimeKind.Local).AddTicks(4014));

            migrationBuilder.UpdateData(
                table: "SottoCategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataInserimentoSottoCategoria",
                value: new DateTime(2024, 12, 2, 17, 21, 51, 173, DateTimeKind.Local).AddTicks(4351));

            migrationBuilder.UpdateData(
                table: "SottoCategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataInserimentoSottoCategoria",
                value: new DateTime(2024, 12, 2, 17, 21, 51, 173, DateTimeKind.Local).AddTicks(4451));

            migrationBuilder.UpdateData(
                table: "SottoCategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataInserimentoSottoCategoria",
                value: new DateTime(2024, 12, 2, 17, 21, 51, 173, DateTimeKind.Local).AddTicks(4551));

            migrationBuilder.UpdateData(
                table: "SottoCategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataInserimentoSottoCategoria",
                value: new DateTime(2024, 12, 2, 17, 21, 51, 173, DateTimeKind.Local).AddTicks(4659));

            migrationBuilder.AddForeignKey(
                name: "FK_Documenti_SottoCategoriaDocumenti_SottoCategoriaDocumentiId",
                table: "Documenti",
                column: "SottoCategoriaDocumentiId",
                principalTable: "SottoCategoriaDocumenti",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SottoCategoriaDocumenti_CategoriaDocumenti_CategoriaDocument~",
                table: "SottoCategoriaDocumenti",
                column: "CategoriaDocumentiId",
                principalTable: "CategoriaDocumenti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documenti_SottoCategoriaDocumenti_SottoCategoriaDocumentiId",
                table: "Documenti");

            migrationBuilder.DropForeignKey(
                name: "FK_SottoCategoriaDocumenti_CategoriaDocumenti_CategoriaDocument~",
                table: "SottoCategoriaDocumenti");

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataInserimentoCategoria",
                value: new DateTime(2024, 11, 29, 13, 34, 5, 793, DateTimeKind.Local).AddTicks(3366));

            migrationBuilder.UpdateData(
                table: "CategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataInserimentoCategoria",
                value: new DateTime(2024, 11, 29, 13, 34, 5, 793, DateTimeKind.Local).AddTicks(3458));

            migrationBuilder.UpdateData(
                table: "SottoCategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 1,
                column: "DataInserimentoSottoCategoria",
                value: new DateTime(2024, 11, 29, 13, 34, 5, 793, DateTimeKind.Local).AddTicks(3746));

            migrationBuilder.UpdateData(
                table: "SottoCategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 2,
                column: "DataInserimentoSottoCategoria",
                value: new DateTime(2024, 11, 29, 13, 34, 5, 793, DateTimeKind.Local).AddTicks(3837));

            migrationBuilder.UpdateData(
                table: "SottoCategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 3,
                column: "DataInserimentoSottoCategoria",
                value: new DateTime(2024, 11, 29, 13, 34, 5, 793, DateTimeKind.Local).AddTicks(3927));

            migrationBuilder.UpdateData(
                table: "SottoCategoriaDocumenti",
                keyColumn: "Id",
                keyValue: 4,
                column: "DataInserimentoSottoCategoria",
                value: new DateTime(2024, 11, 29, 13, 34, 5, 793, DateTimeKind.Local).AddTicks(4016));

            migrationBuilder.AddForeignKey(
                name: "FK_Documenti_SottoCategoriaDocumenti_SottoCategoriaDocumentiId",
                table: "Documenti",
                column: "SottoCategoriaDocumentiId",
                principalTable: "SottoCategoriaDocumenti",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_SottoCategoriaDocumenti_CategoriaDocumenti_CategoriaDocument~",
                table: "SottoCategoriaDocumenti",
                column: "CategoriaDocumentiId",
                principalTable: "CategoriaDocumenti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
