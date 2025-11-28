namespace ProyectoGatitos.Models.AdopcionDTOs
{
    public class EditarProductoDTO
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal? Precio { get; set; }
        public string? Descripcion { get; set; }
    }
}
