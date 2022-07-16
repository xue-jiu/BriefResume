using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BriefResume.Migrations
{
    public partial class NewKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ablities_seekerAttributes_JobSeekerId",
                table: "ablities");

            migrationBuilder.DropForeignKey(
                name: "FK_interviewerAttributes_AspNetUsers_InterviewerId",
                table: "interviewerAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_seekerAttributes_AspNetUsers_JobSeekerId",
                table: "seekerAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_seekerAttributes",
                table: "seekerAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_interviewerAttributes",
                table: "interviewerAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ablities",
                table: "ablities");

            migrationBuilder.DropColumn(
                name: "JobSeekerId",
                table: "ablities");

            migrationBuilder.AlterColumn<string>(
                name: "JobSeekerId",
                table: "seekerAttributes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "SeekerAttributeId",
                table: "seekerAttributes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "InterviewerId",
                table: "interviewerAttributes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<Guid>(
                name: "InterviewerAttributeId",
                table: "interviewerAttributes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AblityId",
                table: "ablities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "SeekerAttributeId",
                table: "ablities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_seekerAttributes",
                table: "seekerAttributes",
                column: "SeekerAttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_interviewerAttributes",
                table: "interviewerAttributes",
                column: "InterviewerAttributeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ablities",
                table: "ablities",
                column: "AblityId");

            migrationBuilder.CreateIndex(
                name: "IX_seekerAttributes_JobSeekerId",
                table: "seekerAttributes",
                column: "JobSeekerId",
                unique: true,
                filter: "[JobSeekerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_interviewerAttributes_InterviewerId",
                table: "interviewerAttributes",
                column: "InterviewerId",
                unique: true,
                filter: "[InterviewerId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ablities_SeekerAttributeId",
                table: "ablities",
                column: "SeekerAttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ablities_seekerAttributes_SeekerAttributeId",
                table: "ablities",
                column: "SeekerAttributeId",
                principalTable: "seekerAttributes",
                principalColumn: "SeekerAttributeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_interviewerAttributes_AspNetUsers_InterviewerId",
                table: "interviewerAttributes",
                column: "InterviewerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_seekerAttributes_AspNetUsers_JobSeekerId",
                table: "seekerAttributes",
                column: "JobSeekerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ablities_seekerAttributes_SeekerAttributeId",
                table: "ablities");

            migrationBuilder.DropForeignKey(
                name: "FK_interviewerAttributes_AspNetUsers_InterviewerId",
                table: "interviewerAttributes");

            migrationBuilder.DropForeignKey(
                name: "FK_seekerAttributes_AspNetUsers_JobSeekerId",
                table: "seekerAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_seekerAttributes",
                table: "seekerAttributes");

            migrationBuilder.DropIndex(
                name: "IX_seekerAttributes_JobSeekerId",
                table: "seekerAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_interviewerAttributes",
                table: "interviewerAttributes");

            migrationBuilder.DropIndex(
                name: "IX_interviewerAttributes_InterviewerId",
                table: "interviewerAttributes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ablities",
                table: "ablities");

            migrationBuilder.DropIndex(
                name: "IX_ablities_SeekerAttributeId",
                table: "ablities");

            migrationBuilder.DropColumn(
                name: "SeekerAttributeId",
                table: "seekerAttributes");

            migrationBuilder.DropColumn(
                name: "InterviewerAttributeId",
                table: "interviewerAttributes");

            migrationBuilder.DropColumn(
                name: "AblityId",
                table: "ablities");

            migrationBuilder.DropColumn(
                name: "SeekerAttributeId",
                table: "ablities");

            migrationBuilder.AlterColumn<string>(
                name: "JobSeekerId",
                table: "seekerAttributes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InterviewerId",
                table: "interviewerAttributes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobSeekerId",
                table: "ablities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_seekerAttributes",
                table: "seekerAttributes",
                column: "JobSeekerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_interviewerAttributes",
                table: "interviewerAttributes",
                column: "InterviewerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ablities",
                table: "ablities",
                column: "JobSeekerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ablities_seekerAttributes_JobSeekerId",
                table: "ablities",
                column: "JobSeekerId",
                principalTable: "seekerAttributes",
                principalColumn: "JobSeekerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_interviewerAttributes_AspNetUsers_InterviewerId",
                table: "interviewerAttributes",
                column: "InterviewerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_seekerAttributes_AspNetUsers_JobSeekerId",
                table: "seekerAttributes",
                column: "JobSeekerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
