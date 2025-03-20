using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_tbl_UserToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceId",
                table: "UserTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "UserTokens");
        }
    }
}
