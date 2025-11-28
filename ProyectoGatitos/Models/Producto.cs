namespace ProyectoGatitos.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio {  get; set; }
        public bool EnStock { get; set; }
        public List<string>? Fotos { get; set; } = new List<string>();
        public string? Descripcion { get; set; }
    }
}
