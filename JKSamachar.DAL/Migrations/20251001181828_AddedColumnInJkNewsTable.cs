using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JKSamachar.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumnInJkNewsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdminedAllowPublish",
                table: "JKNews",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdminedAllowPublish",
                table: "JKNews");
        }
    }
}
