using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajApi.Migrations
{
    /// <inheritdoc />
    public partial class AddKaanSeedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), "kaan.kesen@pointr.tech", "Kaan" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
