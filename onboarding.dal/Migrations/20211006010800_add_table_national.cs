using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace onboarding.dal.Migrations
{
    public partial class add_table_national : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Duration",
                table: "Movie",
                newName: "ImdbId");

            migrationBuilder.AddColumn<Guid>(
                name: "NationalId",
                table: "Movie",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "National",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_National", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movie_NationalId",
                table: "Movie",
                column: "NationalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_National_NationalId",
                table: "Movie",
                column: "NationalId",
                principalTable: "National",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_National_NationalId",
                table: "Movie");

            migrationBuilder.DropTable(
                name: "National");

            migrationBuilder.DropIndex(
                name: "IX_Movie_NationalId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "Movie");

            migrationBuilder.RenameColumn(
                name: "ImdbId",
                table: "Movie",
                newName: "Duration");
        }
    }
}
