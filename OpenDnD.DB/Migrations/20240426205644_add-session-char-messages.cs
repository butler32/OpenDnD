using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenDnD.DB.Migrations
{
    /// <inheritdoc />
    public partial class addsessioncharmessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionChatMessages",
                columns: table => new
                {
                    SessionChatMessageId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SessionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionChatMessages", x => x.SessionChatMessageId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionChatMessages");
        }
    }
}
