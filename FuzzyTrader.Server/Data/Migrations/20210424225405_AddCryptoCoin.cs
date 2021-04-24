using Microsoft.EntityFrameworkCore.Migrations;

namespace FuzzyTrader.Server.Data.Migrations
{
    public partial class AddCryptoCoin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssetId",
                table: "Investments",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Investments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CryptoCoins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AssetId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    TypeIsCrypto = table.Column<bool>(type: "boolean", nullable: false),
                    DataQuoteStart = table.Column<string>(type: "text", nullable: true),
                    DataQuoteEnd = table.Column<string>(type: "text", nullable: true),
                    DataOrderbookStart = table.Column<string>(type: "text", nullable: true),
                    DataOrderbookEnd = table.Column<string>(type: "text", nullable: true),
                    DataTradeStart = table.Column<string>(type: "text", nullable: true),
                    DataTradeEnd = table.Column<string>(type: "text", nullable: true),
                    DataQuoteCount = table.Column<string>(type: "text", nullable: true),
                    DataTradeCount = table.Column<string>(type: "text", nullable: true),
                    DataSymbolsCount = table.Column<string>(type: "text", nullable: true),
                    Volume1HrsUsd = table.Column<string>(type: "text", nullable: true),
                    Volume1DayUsd = table.Column<string>(type: "text", nullable: true),
                    Volume1MthUsd = table.Column<string>(type: "text", nullable: true),
                    PriceUsd = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoCoins", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptoCoins");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Investments");
        }
    }
}
