using Microsoft.EntityFrameworkCore.Migrations;

namespace FuzzyTrader.Server.Data.Migrations
{
    public partial class addTradeAssetTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeAssets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Open = table.Column<decimal>(type: "numeric", nullable: true),
                    High = table.Column<decimal>(type: "numeric", nullable: true),
                    Low = table.Column<decimal>(type: "numeric", nullable: true),
                    Close = table.Column<decimal>(type: "numeric", nullable: true),
                    Volume = table.Column<decimal>(type: "numeric", nullable: true),
                    AdjHigh = table.Column<decimal>(type: "numeric", nullable: true),
                    AdjLow = table.Column<decimal>(type: "numeric", nullable: true),
                    AdjClose = table.Column<decimal>(type: "numeric", nullable: true),
                    AdjOpen = table.Column<decimal>(type: "numeric", nullable: true),
                    AdjVolume = table.Column<decimal>(type: "numeric", nullable: true),
                    SplitFactor = table.Column<decimal>(type: "numeric", nullable: true),
                    Symbol = table.Column<string>(type: "text", nullable: true),
                    Exchange = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeAssets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeAssets");
        }
    }
}
