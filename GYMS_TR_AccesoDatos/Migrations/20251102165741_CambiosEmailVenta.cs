using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYMS_TR_AccesoDatos.Migrations
{
    public partial class CambiosEmailVenta : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emial",
                table: "Venta");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Venta",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Venta");

            migrationBuilder.AddColumn<string>(
                name: "Emial",
                table: "Venta",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
