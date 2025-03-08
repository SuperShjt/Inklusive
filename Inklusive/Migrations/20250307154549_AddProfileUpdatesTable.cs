using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inklusive.Migrations
{
    /// <inheritdoc />
    public partial class AddProfileUpdatesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfileUpdates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UpdatedField = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    DateRequested = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateApproved = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileUpdates_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileUpdates_UserId",
                table: "ProfileUpdates",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileUpdates");
        }
    }
}
