using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDnD.DB.Migrations
{
    /// <inheritdoc />
    public partial class addsessioncharmessages2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SessionChatMessages_PlayerId",
                table: "SessionChatMessages",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionChatMessages_SessionId",
                table: "SessionChatMessages",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCharacters_PlayerId",
                table: "PlayerCharacters",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerCharacters_Players_PlayerId",
                table: "PlayerCharacters",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionChatMessages_Players_PlayerId",
                table: "SessionChatMessages",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "PlayerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionChatMessages_Sessions_SessionId",
                table: "SessionChatMessages",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "SessionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerCharacters_Players_PlayerId",
                table: "PlayerCharacters");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionChatMessages_Players_PlayerId",
                table: "SessionChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionChatMessages_Sessions_SessionId",
                table: "SessionChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_SessionChatMessages_PlayerId",
                table: "SessionChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_SessionChatMessages_SessionId",
                table: "SessionChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_PlayerCharacters_PlayerId",
                table: "PlayerCharacters");
        }
    }
}
