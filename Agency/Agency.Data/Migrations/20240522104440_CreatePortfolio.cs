using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Agency.Data.Migrations
{
    public partial class CreatePortfolio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_portfolios",
                table: "portfolios");

            migrationBuilder.RenameTable(
                name: "portfolios",
                newName: "Portfoliooss");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Portfoliooss",
                table: "Portfoliooss",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Portfoliooss",
                table: "Portfoliooss");

            migrationBuilder.RenameTable(
                name: "Portfoliooss",
                newName: "portfolios");

            migrationBuilder.AddPrimaryKey(
                name: "PK_portfolios",
                table: "portfolios",
                column: "Id");
        }
    }
}
