using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "tbGame",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "varchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbGame", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbMember",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbMember", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbMembertbGame",
                schema: "dbo",
                columns: table => new
                {
                    IdMember = table.Column<int>(type: "int", nullable: false),
                    IdGame = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbMembertbGame", x => new { x.IdMember, x.IdGame });
                    table.ForeignKey(
                        name: "FK_tbMembertbGame_Game",
                        column: x => x.IdGame,
                        principalSchema: "dbo",
                        principalTable: "tbGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbMembertbGame_Member",
                        column: x => x.IdMember,
                        principalSchema: "dbo",
                        principalTable: "tbMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbMember_Email",
                schema: "dbo",
                table: "tbMember",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbMembertbGame_IdGame",
                schema: "dbo",
                table: "tbMembertbGame",
                column: "IdGame");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbMembertbGame",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tbGame",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tbMember",
                schema: "dbo");
        }
    }
}
