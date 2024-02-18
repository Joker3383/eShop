using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Order.API.Migrations
{
    /// <inheritdoc />
    public partial class init5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Orders_OrderId",
                table: "ShoppingCarts");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_Products_ProductId",
                table: "ShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCarts",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_OrderId",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "Login",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "ShoppingCarts",
                newName: "ShoppingCartDto");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCarts_ProductId",
                table: "ShoppingCartDto",
                newName: "IX_ShoppingCartDto_ProductId");

            migrationBuilder.AddColumn<int>(
                name: "BasketId",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubId",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartDto",
                table: "ShoppingCartDto",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartDto_Products_ProductId",
                table: "ShoppingCartDto",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartDto_Products_ProductId",
                table: "ShoppingCartDto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartDto",
                table: "ShoppingCartDto");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SubId",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "ShoppingCartDto",
                newName: "ShoppingCarts");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartDto_ProductId",
                table: "ShoppingCarts",
                newName: "IX_ShoppingCarts_ProductId");

            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCarts",
                table: "ShoppingCarts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_OrderId",
                table: "ShoppingCarts",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Orders_OrderId",
                table: "ShoppingCarts",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_Products_ProductId",
                table: "ShoppingCarts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
