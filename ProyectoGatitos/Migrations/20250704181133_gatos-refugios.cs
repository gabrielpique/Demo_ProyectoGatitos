using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoGatitos.Migrations
{
    /// <inheritdoc />
    public partial class gatosrefugios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactoRefugio",
                table: "Adopciones",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EsDeRefugio",
                table: "Adopciones",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NombreRefugio",
                table: "Adopciones",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactoRefugio",
                table: "Adopciones");

            migrationBuilder.DropColumn(
                name: "EsDeRefugio",
                table: "Adopciones");

            migrationBuilder.DropColumn(
                name: "NombreRefugio",
                table: "Adopciones");
        }
    }
}
