using Microsoft.AspNetCore.Identity;
using ProyectoGatitos.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoGatitos.Models
{
    public class Adopcion
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime FechaPublicacion { get; set; } 
        public DateTime? FechaDeAdopcion { get; set; }
        public string? Edad { get; set; }
        public SexoEnum Sexo { get; set; }
        public string? SexoPersonalizado { get; set; } = "Otro";
        public string Descripcion { get; set; } = "Sin descripción";
        public bool Adoptado { get; set; } = false;
        public string? PersonaQueAdopto { get; set; }
        public string? NumeroContacto { get; set; }
        public int CantidadGatos { get; set; } = 1;
        public List<string>? Fotos { get; set; } = new List<string>();
        public string? UsuarioPublicadorId { get; set; }

        [ForeignKey("UsuarioPublicadorId")]
        public ApplicationUser? UsuarioPublicador { get; set; }

        // Nuevos campos para adopciones de otros refugios
        public string? NombreRefugio { get; set; } 
        public string? ContactoRefugio { get; set; } 
        public bool EsDeRefugio { get; set; } = false;

    }
}