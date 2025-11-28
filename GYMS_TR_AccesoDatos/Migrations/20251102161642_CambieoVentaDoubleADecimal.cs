using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYMS_TR_AccesoDatos.Migrations
{
    public partial class CambieoVentaDoubleADecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "FinalVentaTotal",
                table: "Venta",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "FinalVentaTotal",
                table: "Venta",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
