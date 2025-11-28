using System.ComponentModel.DataAnnotations;

namespace ProyectoGatitos.Models.AdopcionDTOs
{
    public class ProductoDTO
    {
        [Required]
        public string Nombre { get; set; }

        [Range(0, 999999)]
        public decimal Precio { get; set; }

        public bool EnStock { get; set; }

        [StringLength(500)]
        public string? Descripcion { get; set; }

        public List<IFormFile>? Fotos { get; set; } = new List<IFormFile>();

    }
}
