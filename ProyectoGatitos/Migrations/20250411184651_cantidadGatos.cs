using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoGatitos.Migrations
{
    /// <inheritdoc />
    public partial class cantidadGatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadGatos",
                table: "Adopciones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadGatos",
                table: "Adopciones");
        }
    }
}
