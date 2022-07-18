using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BriefResume.Migrations
{
    public partial class AddInitRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeekerId",
                table: "AspNetUserRoles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "308660dc-ae51-480f-824d-7dca6714c3e2", "73f3fc45-8d4a-4dd9-90c9-3e780d311136", "superMoster", "SUPERMOSTER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreateDatetime", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Preference", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "90184155-dee0-40c9-bb1e-b5ed07afc04e", 0, "8661eda6-c31b-40b1-89c1-374908efe8bf", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "adminNanaliz@qq.com", true, false, null, "ADMINNANALIZ@QQ.COM", "ADMINNANALIZ@QQ.COM", "AQAAAAEAACcQAAAAEH0bSUqT34Zmf+JYqpU+ffTcgJ1P9q+wozEWN82W3YAJ3ogDEEiLIlwd7pmuFBHr7Q==", "123456789", false, null, "d2f276e4-1be4-453c-b384-fa3b292bbeff", false, "adminNanaliz@qq.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId", "SeekerId" },
                values: new object[] { "308660dc-ae51-480f-824d-7dca6714c3e2", "90184155-dee0-40c9-bb1e-b5ed07afc04e", null });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_SeekerId",
                table: "AspNetUserRoles",
                column: "SeekerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_SeekerId",
                table: "AspNetUserRoles",
                column: "SeekerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_SeekerId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_SeekerId",
                table: "AspNetUserRoles");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "308660dc-ae51-480f-824d-7dca6714c3e2", "90184155-dee0-40c9-bb1e-b5ed07afc04e" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e");

            migrationBuilder.DropColumn(
                name: "SeekerId",
                table: "AspNetUserRoles");
        }
    }
}
