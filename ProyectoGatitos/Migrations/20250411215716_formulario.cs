using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoGatitos.Migrations
{
    /// <inheritdoc />
    public partial class formulario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Formularios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroContacto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotivoAdopcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RedesSociales = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localidad = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Provincia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoPostal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoVivienda = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaracteristicasHogar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RedesProteccion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AprobacionFamiliar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QueHariasSiTeMudás = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TieneMascotas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DetallesMascotas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpinionEsterilizacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MascotasCastradas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedidasAdaptacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConQuienVive = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiesgoFiestas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarcaAlimento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportanciaAnimal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DispuestoTraslado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComentariosAdicionales = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formularios", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Formularios");
        }
    }
}
