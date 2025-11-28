using ProyectoGatitos.Repositories;
using Microsoft.EntityFrameworkCore;
using ProyectoGatitos.Models;
using ProyectoGatitos.Models.AdopcionDTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ProyectoGatitos.Services
{
    public class AdopcionesService
    {
        private readonly IEntityBaseRepository<Adopcion> _adopcionesRepository;
        private readonly IEntityBaseRepository<Formulario> _formulariosRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdopcionesService(IEntityBaseRepository<Adopcion> adopcionesRepository, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment, IEntityBaseRepository<Formulario> formulariosRepository)
        {
            _adopcionesRepository = adopcionesRepository;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _formulariosRepository = formulariosRepository;
        }

        public async Task<List<Adopcion>> GetAllAdopciones()
        {
            return await _adopcionesRepository.GetAll()
                .ToListAsync();
        }


        public async Task<Adopcion> GetAdopcionById(int id)
        {
            return await _adopcionesRepository.GetSingleAsync(a => a.Id == id);
        }
        public async Task<List<Adopcion>> GetAdopcionesPendientes(int pageNumber, int pageSize)
        {
            return await _adopcionesRepository.GetAll()
                .Where(a => !a.Adoptado)
                .Where(a => !a.EsDeRefugio)
                .OrderByDescending(a => a.FechaPublicacion)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<List<Adopcion>> GetAdopcionesPendientesRefugiosExternos(int pageNumber, int pageSize)
        {
            return await _adopcionesRepository.GetAll()
                .Where(a => !a.Adoptado)
                .Where(a => a.EsDeRefugio)
                .OrderByDescending(a => a.FechaPublicacion)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int> GetCantidadGatosPendientesDeAdopcion()
        {
            return await _adopcionesRepository.GetAll()
                .Where(a => !a.Adoptado)
                .Where(a => !a.EsDeRefugio)
                .SumAsync(a => a.CantidadGatos);
        }
        public async Task<int> GetCantidadGatosPendientesDeAdopcionRefugio()
        {
            return await _adopcionesRepository.GetAll()
                .Where(a => !a.Adoptado)
                .Where(a => a.EsDeRefugio)
                .SumAsync(a => a.CantidadGatos);
        }

        public async Task<int> GetCantidadGatosAdoptados()
        {
            return await _adopcionesRepository.GetAll()
                .Where(a => a.Adoptado)
                .Where(a => !a.EsDeRefugio)
                .SumAsync(a => a.CantidadGatos);
        }
        public async Task<int> GetCantidadGatosAdoptadosRefugiosExternos()
        {
            return await _adopcionesRepository.GetAll()
                .Where(a => a.Adoptado)
                .Where(a => a.EsDeRefugio)
                .SumAsync(a => a.CantidadGatos);
        }

        public async Task<int> GetTotalAdopcionesPendientes()
        {
            return await _adopcionesRepository.GetAll()
                .Where(a => !a.EsDeRefugio)
                .CountAsync(a => !a.Adoptado);
        }
        public async Task<int> GetTotalAdopcionesRefugioPendientes()
        {
            return await _adopcionesRepository.GetAll()
                .Where(a => a.EsDeRefugio)
                .CountAsync(a => !a.Adoptado);
        }
        
        public async Task<List<Adopcion>> GetAdopcionesRealizadas(int pageNumber, int pageSize)
        {
            return await _adopcionesRepository.GetAll()
                .Where (a => a.Adoptado)
                .Where(a => !a.EsDeRefugio)
                .OrderByDescending(a => a.FechaDeAdopcion)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<List<Adopcion>> GetAdopcionesRealizadasRefugiosExternos(int pageNumber, int pageSize)
        {
            return await _adopcionesRepository.GetAll()
                .Where(a => a.Adoptado)
                .Where(a => a.EsDeRefugio)
                .OrderByDescending(a => a.FechaDeAdopcion)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalAdopcionesRealizadas()
        {
            return await _adopcionesRepository.GetAll().Where(a => !a.EsDeRefugio)
                .CountAsync(a => a.Adoptado);
        }
        public async Task<int> GetTotalAdopcionesRealizadasRefugiosExternos()
        {
            return await _adopcionesRepository.GetAll().Where(a => a.EsDeRefugio)
                .CountAsync(a => a.Adoptado);
        }
        public async Task<List<Formulario>> GetAllFormularios()
        {
            return await _formulariosRepository.GetAll()
                .OrderBy(f => f.Revisado)                  
                .ThenByDescending(f => f.FechaSolicitud)   
                .ToListAsync();
        }
        public async Task<int> GetCountFormulariosSinRevisar()
        {
            var formulariosSinRevisar = await _formulariosRepository.GetAll()
                .Where(f => !f.Revisado)
                .CountAsync();

            return formulariosSinRevisar;
        }

        public async Task<Formulario> GetFormularioById(int id)
        {
            return await _formulariosRepository.GetSingleAsync(f => f.Id == id);
        }

        public async Task<Adopcion> CreateAdopcion(AdopcionDTO adopcionDto)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var nuevaAdopcion = new Adopcion
            {
                Nombre = adopcionDto.Nombre,
                FechaPublicacion = DateTime.UtcNow,
                Edad = adopcionDto.Edad,
                Sexo = adopcionDto.Sexo,
                SexoPersonalizado = adopcionDto.SexoPersonalizado,
                CantidadGatos = adopcionDto.CantidadGatos,
                Descripcion = adopcionDto.Descripcion,
                Adoptado = false,
                UsuarioPublicadorId = userId,
                Fotos = new List<string>(),
                EsDeRefugio = adopcionDto.EsDeRefugio,
                NombreRefugio = adopcionDto.NombreRefugio,
                ContactoRefugio = adopcionDto.ContactoRefugio
            };

            if (adopcionDto.Fotos != null && adopcionDto.Fotos.Any())
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var formatosPermitidos = new List<string> { ".jpg", ".jpeg", ".png" };
                const int maxSizeInBytes = 6 * 1024 * 1024; // 6MB

                foreach (var foto in adopcionDto.Fotos)
                {
                    if (foto != null)
                    {
                        var extension = Path.GetExtension(foto.FileName).ToLower();
                        if (!formatosPermitidos.Contains(extension))
                        {
                            throw new InvalidOperationException("Formato de imagen no permitido.");
                        }

                        if (foto.Length > maxSizeInBytes)
                        {
                            throw new InvalidOperationException("El tamaño máximo de cada imagen es de 2MB.");
                        }

                        var fileName = $"{Guid.NewGuid()}.jpg"; // Convertir todo a jpg comprimido
                        var filePath = Path.Combine(uploadsFolder, fileName);

                        using var image = await Image.LoadAsync(foto.OpenReadStream());

                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(1280, 1280) 
                        }));

                        var encoder = new JpegEncoder
                        {
                            Quality = 75 
                        };

                        await image.SaveAsync(filePath, encoder);

                        nuevaAdopcion.Fotos.Add($"/uploads/{fileName}");
                    }
                }

            }

            _adopcionesRepository.Add(nuevaAdopcion);
            await _adopcionesRepository.SaveChangesAsync();

            return nuevaAdopcion;
        }

        public async Task<Formulario> EnviarFormulario(Formulario formulario)
        {
            _formulariosRepository.Add(formulario);
            await _formulariosRepository.SaveChangesAsync();
            return formulario;
        }

        public async Task<Formulario> MarcarComoRevisado(int id)
        {
            var formulario = await _formulariosRepository.GetSingleAsync(f => f.Id == id);
            if(formulario == null)
            {
                throw new KeyNotFoundException($"Formulario no encontrado");
            }

            formulario.Revisado = true;
            _formulariosRepository.Update(formulario);
            await _formulariosRepository.SaveChangesAsync();
            return formulario;
        }

        public async Task<Adopcion> ActualizarAdopcion(AdopcionDTO adopcionDto)
        {
            var adopcionExistente = await _adopcionesRepository.GetSingleAsync(a => a.Id == adopcionDto.Id);
            if (adopcionExistente == null)
            {
                throw new KeyNotFoundException($"Adopción con Id {adopcionDto.Id} no encontrada.");
            }

            adopcionExistente.Nombre = adopcionDto.Nombre ?? adopcionExistente.Nombre;
            adopcionExistente.Descripcion = adopcionDto.Descripcion ?? adopcionExistente.Descripcion;
            adopcionExistente.PersonaQueAdopto = adopcionDto.PersonaQueAdopto ?? adopcionExistente.PersonaQueAdopto;
            adopcionExistente.Edad = adopcionDto.Edad ?? adopcionExistente.Edad;
            adopcionExistente.Sexo = adopcionDto.Sexo;
            adopcionExistente.CantidadGatos = adopcionDto.CantidadGatos;
            adopcionExistente.SexoPersonalizado = adopcionDto.SexoPersonalizado;
            adopcionExistente.NombreRefugio = adopcionDto.NombreRefugio;
            adopcionExistente.ContactoRefugio = adopcionDto.ContactoRefugio;

            _adopcionesRepository.Update(adopcionExistente);
            await _adopcionesRepository.SaveChangesAsync();

            return adopcionExistente;
        }

        public async Task<bool> DeleteAdopcion(int id)
        {
            var adopcionExistente = await _adopcionesRepository.GetSingleAsync(a => a.Id == id);
            if (adopcionExistente == null)
            {
                return false; 
            }

            _adopcionesRepository.Delete(adopcionExistente);
            await _adopcionesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ConfirmarAdopcion(int id, string personaQueAdopto)
        {
            var adopcion = await _adopcionesRepository.GetSingleAsync(a => a.Id == id);
            if(adopcion == null)
            {
                return false;
            }

            adopcion.Adoptado = true;
            adopcion.PersonaQueAdopto = personaQueAdopto;
            adopcion.FechaDeAdopcion = DateTime.UtcNow;

            await _adopcionesRepository.SaveChangesAsync(); 
            
            return true;
        }


    }
}