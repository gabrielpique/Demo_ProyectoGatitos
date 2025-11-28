using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoGatitos.Migrations
{
    /// <inheritdoc />
    public partial class sexo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SexoPersonalizado",
                table: "Adopciones",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SexoPersonalizado",
                table: "Adopciones");
        }
    }
}
