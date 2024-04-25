using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDnD.DB.Migrations
{
    /// <inheritdoc />
    public partial class addplayerCharacter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerCharacters",
                columns: table => new
                {
                    PlayerCharacterId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerCharacterName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCharacters", x => x.PlayerCharacterId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerCharacters");
        }
    }
}
