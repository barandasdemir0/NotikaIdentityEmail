using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotikaIdentityEmail.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMailColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiverEail",
                table: "Messages",
                newName: "ReceiverEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiverEmail",
                table: "Messages",
                newName: "ReceiverEail");
        }
    }
}
