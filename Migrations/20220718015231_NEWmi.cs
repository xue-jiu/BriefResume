using Microsoft.EntityFrameworkCore.Migrations;

namespace BriefResume.Migrations
{
    public partial class NEWmi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "c78c4e8a-17bc-4aec-a5ca-d721e79c7b6b");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4822ef37-184b-4c51-82c4-1ef53fb0fe32", "AQAAAAEAACcQAAAAEKOi9bLGkCdrek0oQJU/qOLXxQju+ujYQR98AkbGk0gqH8gEiA+KG5zc0Wx9A+HqQQ==", "243336cf-0bac-4710-800a-43ffef8cf63c" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "308660dc-ae51-480f-824d-7dca6714c3e2",
                column: "ConcurrencyStamp",
                value: "73f3fc45-8d4a-4dd9-90c9-3e780d311136");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "90184155-dee0-40c9-bb1e-b5ed07afc04e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8661eda6-c31b-40b1-89c1-374908efe8bf", "AQAAAAEAACcQAAAAEH0bSUqT34Zmf+JYqpU+ffTcgJ1P9q+wozEWN82W3YAJ3ogDEEiLIlwd7pmuFBHr7Q==", "d2f276e4-1be4-453c-b384-fa3b292bbeff" });
        }
    }
}
