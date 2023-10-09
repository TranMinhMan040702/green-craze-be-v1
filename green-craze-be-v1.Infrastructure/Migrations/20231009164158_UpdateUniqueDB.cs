using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace green_craze_be_v1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUniqueDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Variants_Sku",
                table: "Variants");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Units_Name",
                table: "Units");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Transactions_PaypalOrderId",
                table: "Transactions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Products_Code",
                table: "Products");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Products_Name",
                table: "Products");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Products_Slug",
                table: "Products");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProductCategories_Name",
                table: "ProductCategories");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProductCategories_Slug",
                table: "ProductCategories");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PaymentMethods_Code",
                table: "PaymentMethods");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_PaymentMethods_Name",
                table: "PaymentMethods");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Orders_Code",
                table: "Orders");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_OrderCancellationReasons_Name",
                table: "OrderCancellationReasons");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Deliveries_Name",
                table: "Deliveries");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Brands_Code",
                table: "Brands");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Brands_Name",
                table: "Brands");

            migrationBuilder.AlterColumn<string>(
                name: "Sku",
                table: "Variants",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Units",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Products",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductCategories",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PaymentMethods",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "PaymentMethods",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Orders",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OrderCancellationReasons",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Deliveries",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Brands",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Brands",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)");

            migrationBuilder.CreateIndex(
                name: "IX_Variants_Sku",
                table: "Variants",
                column: "Sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Units_Name",
                table: "Units",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_PaypalOrderId",
                table: "Transactions",
                column: "PaypalOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code",
                table: "Products",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Slug",
                table: "Products",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_Name",
                table: "ProductCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_Slug",
                table: "ProductCategories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Code",
                table: "PaymentMethods",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Name",
                table: "PaymentMethods",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Code",
                table: "Orders",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderCancellationReasons_Name",
                table: "OrderCancellationReasons",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_Name",
                table: "Deliveries",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Code",
                table: "Brands",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Name",
                table: "Brands",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Variants_Sku",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Units_Name",
                table: "Units");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_PaypalOrderId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Products_Code",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Name",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_Slug",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_Name",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductCategories_Slug",
                table: "ProductCategories");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_Code",
                table: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_Name",
                table: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Code",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_OrderCancellationReasons_Name",
                table: "OrderCancellationReasons");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_Name",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Brands_Code",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Brands_Name",
                table: "Brands");

            migrationBuilder.AlterColumn<string>(
                name: "Sku",
                table: "Variants",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Units",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Products",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductCategories",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PaymentMethods",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "PaymentMethods",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Orders",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OrderCancellationReasons",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Deliveries",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Brands",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Brands",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Variants_Sku",
                table: "Variants",
                column: "Sku");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Units_Name",
                table: "Units",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Transactions_PaypalOrderId",
                table: "Transactions",
                column: "PaypalOrderId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Products_Code",
                table: "Products",
                column: "Code");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Products_Slug",
                table: "Products",
                column: "Slug");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProductCategories_Name",
                table: "ProductCategories",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProductCategories_Slug",
                table: "ProductCategories",
                column: "Slug");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PaymentMethods_Code",
                table: "PaymentMethods",
                column: "Code");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_PaymentMethods_Name",
                table: "PaymentMethods",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Orders_Code",
                table: "Orders",
                column: "Code");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_OrderCancellationReasons_Name",
                table: "OrderCancellationReasons",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Deliveries_Name",
                table: "Deliveries",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Brands_Code",
                table: "Brands",
                column: "Code");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Brands_Name",
                table: "Brands",
                column: "Name");
        }
    }
}
