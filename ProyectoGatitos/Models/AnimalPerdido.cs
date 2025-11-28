using ProyectoGatitos.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoGatitos.Models
{
    public class AnimalPerdido
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public DateTime FechaEnQueSePerdio { get; set; }
        public DateTime FechaPublicacion {  get; set; }
        public List<string> FotosDelAnimal { get; set; } = new List<string>();
        public string? Mensaje { get; set; }
        public string? Ubicacion { get; set; }
        public bool Recuperado { get; set; } = false;
        public string? NumeroContacto { get; set; }
        public TipoPerdida TipoPerdida { get; set; }
        public string? UsuarioPublicadorId { get; set; }

        [ForeignKey("UsuarioPublicadorId")]
        public ApplicationUser? UsuarioPublicador { get; set; }
    }
}