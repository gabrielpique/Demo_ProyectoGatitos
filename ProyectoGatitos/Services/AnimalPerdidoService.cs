using ProyectoGatitos.Repositories;
using Microsoft.EntityFrameworkCore;
using ProyectoGatitos.Models;
using ProyectoGatitos.Models.AdopcionDTOs;
using System.Security.Claims;

// Por el momento no se utiliza esta funcionalidad
namespace ProyectoGatitos.Services
{
    public class AnimalPerdidoService
    {
        private readonly IEntityBaseRepository<AnimalPerdido> _animalPerdidoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AnimalPerdidoService(IEntityBaseRepository<AnimalPerdido> animalPerdidoRepository, IHttpContextAccessor httpContextAccessor)
        {
            _animalPerdidoRepository = animalPerdidoRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<AnimalPerdido>> GetAllAnimalesPerdidos()
        {
            return await _animalPerdidoRepository.GetAll()
                .Include(a => a.UsuarioPublicador)
                .ToListAsync();
        }

        public async Task<AnimalPerdido?> GetAnimalPerdidoById(int id)
        {
            return await _animalPerdidoRepository.GetSingleAsync(a => a.Id == id);
        }

        public async Task<AnimalPerdido> CreateAnimalPerdido(AnimalPerdidoDTO animalPerdidoDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nuevoAnimal = new AnimalPerdido
            {
                FechaEnQueSePerdio = animalPerdidoDto.FechaEnQueSePerdio ?? DateTime.UtcNow,
                FechaPublicacion = animalPerdidoDto.FechaPublicacion ?? DateTime.UtcNow,
                FotosDelAnimal = animalPerdidoDto.FotosDelAnimal ?? new List<string>(),
                Mensaje = animalPerdidoDto.Mensaje,
                Ubicacion = animalPerdidoDto.Ubicacion,
                Recuperado = animalPerdidoDto.Recuperado,
                NumeroContacto = animalPerdidoDto.NumeroContacto,
                TipoPerdida = animalPerdidoDto.TipoPerdida,
                UsuarioPublicadorId = userId
            };

            _animalPerdidoRepository.Add(nuevoAnimal);
            await _animalPerdidoRepository.SaveChangesAsync();

            return nuevoAnimal;
        }

        public async Task<AnimalPerdido> ActualizarAnimalPerdido(AnimalPerdidoDTO animalPerdidoDto, int id)
        {
            var animalExistente = await _animalPerdidoRepository.GetSingleAsync(a => a.Id == id);
            if (animalExistente == null)
            {
                return null;
            }

            animalExistente.FechaEnQueSePerdio = animalPerdidoDto.FechaEnQueSePerdio ?? animalExistente.FechaEnQueSePerdio;
            animalExistente.FotosDelAnimal = animalPerdidoDto.FotosDelAnimal ?? animalExistente.FotosDelAnimal;
            animalExistente.Mensaje = animalPerdidoDto.Mensaje ?? animalExistente.Mensaje;
            animalExistente.Ubicacion = animalPerdidoDto.Ubicacion ?? animalExistente.Ubicacion;
            animalExistente.Recuperado = animalPerdidoDto.Recuperado;
            animalExistente.NumeroContacto = animalPerdidoDto.NumeroContacto ?? animalExistente.NumeroContacto;
            animalExistente.TipoPerdida = animalPerdidoDto.TipoPerdida;

            _animalPerdidoRepository.Update(animalExistente);
            await _animalPerdidoRepository.SaveChangesAsync();

            return animalExistente;
        }

        public async Task<bool> DeleteAnimalPerdido(int id)
        {
            var animalExistente = await _animalPerdidoRepository.GetSingleAsync(a => a.Id == id);
            if (animalExistente == null)
            {
                return false; 
            }

            _animalPerdidoRepository.Delete(animalExistente);
            await _animalPerdidoRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> YaSeEncontro(int id)
        {
            var animalExistente = await _animalPerdidoRepository.GetSingleAsync(a => a.Id == id);
            if(animalExistente == null)
            {
                return false;
            }

            animalExistente.Recuperado = true;
            await _animalPerdidoRepository.SaveChangesAsync(); 
            
            return true;
        }
    }
}
