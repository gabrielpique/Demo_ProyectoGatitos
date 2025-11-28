using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProyectoGatitos.Models;


namespace ProyectoGatitos.Data
{
    public class DbContextProyectoGatitos : IdentityDbContext<ApplicationUser>
    {
        public DbContextProyectoGatitos(DbContextOptions<DbContextProyectoGatitos> options) : base(options)
        {
        }

        public DbSet<Adopcion> Adopciones { get; set; }
        public DbSet<AnimalPerdido> AnimalesPerdidos { get; set; }
        public DbSet<Formulario> Formularios { get; set; }
        public DbSet<Producto> Productos { get; set; }
    }
}
