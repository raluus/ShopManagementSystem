using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoughtProducts_PaymentDetails_PaymentDetailsId",
                table: "BoughtProducts");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "BoughtProducts");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentDetailsId",
                table: "BoughtProducts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BoughtProducts_PaymentDetails_PaymentDetailsId",
                table: "BoughtProducts",
                column: "PaymentDetailsId",
                principalTable: "PaymentDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BoughtProducts_PaymentDetails_PaymentDetailsId",
                table: "BoughtProducts");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentDetailsId",
                table: "BoughtProducts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "BoughtProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_BoughtProducts_PaymentDetails_PaymentDetailsId",
                table: "BoughtProducts",
                column: "PaymentDetailsId",
                principalTable: "PaymentDetails",
                principalColumn: "Id");
        }
    }
}
