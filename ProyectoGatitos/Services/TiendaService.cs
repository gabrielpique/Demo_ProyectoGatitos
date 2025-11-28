using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ProyectoGatitos.Models;
using ProyectoGatitos.Models.AdopcionDTOs;
using ProyectoGatitos.Repositories;
using System.Collections.Concurrent;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace ProyectoGatitos.Services
{
    public class TiendaService
    {
        private readonly IEntityBaseRepository<Producto> _productosRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TiendaService(
            IEntityBaseRepository<Producto> productosRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _productosRepository = productosRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<Producto>> GetAllProductos()
        {
            return await _productosRepository.GetAll()
                .ToListAsync();
        }

        public async Task<Producto> GetProductoById(int id)
        {
            return await _productosRepository.GetSingleAsync(p => p.Id == id);
        }

        public async Task<Producto> RegistrarProducto(ProductoDTO productoDto)
        {
            var nuevoProducto = new Producto
            {
                Nombre = productoDto.Nombre,
                Precio = productoDto.Precio,
                Descripcion = productoDto.Descripcion,
                EnStock = true,
                Fotos = new List<string>() 
            };

            // CARGAR FOTOS
            if (productoDto.Fotos != null && productoDto.Fotos.Any())
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "productos");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var formatosPermitidos = new List<string> { ".jpg", ".jpeg", ".png" };
                const int maxSizeInBytes = 6 * 1024 * 1024; // 6MB por imagen

                foreach (var foto in productoDto.Fotos)
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
                            throw new InvalidOperationException("El tamaño máximo de cada imagen es de 6MB.");
                        }

                        var fileName = $"{Guid.NewGuid()}.jpg"; // Nombre aleatorio
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

                        nuevoProducto.Fotos.Add($"/productos/{fileName}");
                    }
                }
            }

            _productosRepository.Add(nuevoProducto);
            await _productosRepository.SaveChangesAsync();

            return nuevoProducto;
        }


        public async Task<Producto> EditarProducto(EditarProductoDTO editarProductoDto)
        {
            var productoExistente = await _productosRepository.GetSingleAsync(p => p.Id == editarProductoDto.Id);
            if (productoExistente == null)
            {
                throw new KeyNotFoundException($"Producto no encontrada.");
            }

            productoExistente.Nombre = editarProductoDto.Nombre ?? productoExistente.Nombre;
            productoExistente.Descripcion = editarProductoDto.Descripcion ?? productoExistente.Descripcion;
            productoExistente.Precio = editarProductoDto.Precio ?? productoExistente.Precio;

            _productosRepository.Update(productoExistente);
            await _productosRepository.SaveChangesAsync();

            return productoExistente;
        }

        public async Task<bool> DeleteProducto(int id)
        {
            var productoExistente = await _productosRepository.GetSingleAsync(p => p.Id == id);
            if (productoExistente == null)
            {
                return false;
            }

            _productosRepository.Delete(productoExistente);
            await _productosRepository.SaveChangesAsync();

            return true;
        }
        public async Task<bool> CambiarEstadoStock(int id)
        {
            var producto = await _productosRepository.GetSingleAsync(p => p.Id == id);
            if (producto == null)
            {
                return false;
            }

            if (producto.EnStock)
            {
                producto.EnStock = false;
            }
            else
            {
                producto.EnStock = true;
            }

            _productosRepository.Update(producto);
            await _productosRepository.SaveChangesAsync();

            return true;
        }
            
    }
}
