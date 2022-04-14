using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Test.Scopes.Migrations
{
    public partial class IncludePwdProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 4, 14, 9, 41, 44, 74, DateTimeKind.Local).AddTicks(9100),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 4, 13, 15, 4, 24, 368, DateTimeKind.Local).AddTicks(1361));

            migrationBuilder.AddColumn<byte[]>(
                name: "Password",
                table: "Credentials",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Credentials");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 4, 13, 15, 4, 24, 368, DateTimeKind.Local).AddTicks(1361),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 4, 14, 9, 41, 44, 74, DateTimeKind.Local).AddTicks(9100));
        }
    }
}
