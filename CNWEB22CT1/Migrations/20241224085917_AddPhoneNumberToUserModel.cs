using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNWEB22CT1.Migrations
{
    /// <inheritdoc />
    public partial class AddPhoneNumberToUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Role",
                table: "AspNetUsers",
                newName: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "AspNetUsers",
                newName: "Role");
        }
    }
}
