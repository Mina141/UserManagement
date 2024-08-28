using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.Data.Migrations
{
    public partial class AssignAdminUserToAllRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert Into [Security].[UserRoles]  select 'a9209c44-0085-43c6-9bf6-16b66d517383',Id from [Security].[Roles]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from [Security].[UserRoles] where UserId = 'a9209c44-0085-43c6-9bf6-16b66d517383'");
        }
    }
}
