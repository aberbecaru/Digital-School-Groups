using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolApp.Migrations
{
    public partial class And : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserGroups",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_GroupId",
                table: "UserGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserId",
                table: "UserGroups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_AspNetUsers_UserId",
                table: "UserGroups",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Groups_GroupId",
                table: "UserGroups",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_AspNetUsers_UserId",
                table: "UserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Groups_GroupId",
                table: "UserGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_GroupId",
                table: "UserGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_UserId",
                table: "UserGroups");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserGroups",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
