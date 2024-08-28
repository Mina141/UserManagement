using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.Data.Migrations
{
    public partial class AddAdminUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Security].[Users] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePicture]) VALUES (N'a9209c44-0085-43c6-9bf6-16b66d517383', N'Admin', N'ADMIN', N'Admin@gmail.com', N'ADMIN@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEEdU2pFTsSAp/tUR+b/dDp7+9ukNQZwZiosYS5MeZkS0SvPnkBO2tNCz5oHKwOChPA==', N'CPEDIOQVGP5KPEV4F6SNUSEJ5IP3NOTK', N'b43ed547-46ac-4a87-baf9-028871e38152', NULL, 0, 0, NULL, 1, 0, N'Mina', N'Admin', null)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from [Security].[Users] where Id = 'a9209c44-0085-43c6-9bf6-16b66d517383' ");
        }
    }
}
