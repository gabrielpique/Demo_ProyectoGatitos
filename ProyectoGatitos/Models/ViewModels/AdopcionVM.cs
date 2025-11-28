namespace ProyectoGatitos.Models.ViewModels
{
    public class AdopcionVM
    {
        public List<Adopcion> Adopciones { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string NombreAccion { get; set; } // Desde qué metodo viene la vista porque estoy reutilizando index
        public int CantidadTotalGatos { get; set; }
    }
}
