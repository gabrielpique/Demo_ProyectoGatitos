using ProyectoGatitos.Enums;

namespace ProyectoGatitos.Models.AdopcionDTOs
{
    public class AdopcionDTO
    {
        public int Id { get; set; }
        public string? Nombre {  get; set; }
        public DateTime? FechaDeAdopcion { get; set; }
        public string? Edad { get; set; }
        public int CantidadGatos { get; set; } = 1;
        public SexoEnum Sexo { get; set; }
        public string? SexoPersonalizado { get; set; }
        public string? Descripcion { get; set; }
        public bool Adoptado { get; set; } = false;
        public string? PersonaQueAdopto { get; set; }
        public List<IFormFile>? Fotos { get; set; } = new List<IFormFile>();

        // Nuevos campos para otros refugios
        public string? NombreRefugio { get; set; }
        public string? ContactoRefugio { get; set; }
        public bool EsDeRefugio { get; set; } = false;

    }
}
