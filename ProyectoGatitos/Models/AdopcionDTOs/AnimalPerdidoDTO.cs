using ProyectoGatitos.Enums;

namespace ProyectoGatitos.Models.AdopcionDTOs
{
    public class AnimalPerdidoDTO
    {
        public string? Nombre { get; set; }
        public DateTime? FechaEnQueSePerdio { get; set; }
        public DateTime? FechaPublicacion {  get; set; }
        public List<string> FotosDelAnimal { get; set; } = new List<string>();
        public string? Mensaje { get; set; }
        public string? Ubicacion { get; set; }
        public bool Recuperado { get; set; }
        public string? NumeroContacto { get; set; }
        public TipoPerdida TipoPerdida { get; set; }
    }
}
