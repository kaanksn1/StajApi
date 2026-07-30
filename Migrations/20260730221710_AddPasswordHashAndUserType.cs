using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StajApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordHashAndUserType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "User");

            migrationBuilder.Sql(
                """
                UPDATE "Users"
                SET "PasswordHash" = 'AQAAAAIAAYagAAAAEFzMCYbtMyaU6/phz5IYJm7zfBykYk/pGnV6Zkai/kA7nIh9bCp8TLJs8v694AtN0w=='
                WHERE "PasswordHash" IS NULL;
                """
            );

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "PasswordHash", "UserType" },
                values: new object[] { "AQAAAAIAAYagAAAAEH4nbj6VFxAKSxyW7OdvUWLM/DbLFYNNxKmqZbDxISIfYSonPTZ+TdQUj8DAU8VVJw==", "User" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                columns: new[] { "PasswordHash", "UserType" },
                values: new object[] { "AQAAAAIAAYagAAAAEC3SSIEZyc2COS0QT5qAs7pmAIQY+vBMcD35m5nlqGyZZ8drc66hNOoVX3uIv6BAUA==", "User" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                columns: new[] { "PasswordHash", "UserType" },
                values: new object[] { "AQAAAAIAAYagAAAAEFzMCYbtMyaU6/phz5IYJm7zfBykYk/pGnV6Zkai/kA7nIh9bCp8TLJs8v694AtN0w==", "User" });

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "Users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "Users");
        }
    }
}
