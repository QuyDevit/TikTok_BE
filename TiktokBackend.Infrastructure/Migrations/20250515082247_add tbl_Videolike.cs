using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addtbl_Videolike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VideoLikes",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoLikes", x => new { x.UserId, x.VideoId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VideoLikes");
        }
    }
}
