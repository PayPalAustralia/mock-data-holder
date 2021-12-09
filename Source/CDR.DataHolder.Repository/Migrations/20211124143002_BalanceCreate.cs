using Microsoft.EntityFrameworkCore.Migrations;

namespace CDR.DataHolder.Repository.Migrations
{
    public partial class BalanceCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Balance",
                columns: table => new
                {
                    BalanceId = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "TEXT", nullable: false),
                    AvailableBalance = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreditLimit = table.Column<decimal>(type: "TEXT", nullable: false),
                    AmortisedLimit = table.Column<decimal>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: true),
                    AccountId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balance", x => x.BalanceId);
                    table.ForeignKey(
                        name: "FK_Balance_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BalancePurse",
                columns: table => new
                {
                    BankingBalancePurseId = table.Column<string>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: true),
                    BalanceId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalancePurse", x => x.BankingBalancePurseId);
                    table.ForeignKey(
                        name: "FK_BalancePurse_Balance_BalanceId",
                        column: x => x.BalanceId,
                        principalTable: "Balance",
                        principalColumn: "BalanceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Balance_AccountId",
                table: "Balance",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BalancePurse_BalanceId",
                table: "BalancePurse",
                column: "BalanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BalancePurse");

            migrationBuilder.DropTable(
                name: "Balance");
        }
    }
}
