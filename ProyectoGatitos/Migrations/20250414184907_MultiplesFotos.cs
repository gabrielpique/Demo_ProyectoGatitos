using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoGatitos.Migrations
{
    /// <inheritdoc />
    public partial class MultiplesFotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Foto",
                table: "Productos",
                newName: "Fotos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Fotos",
                table: "Productos",
                newName: "Foto");
        }
    }
}
