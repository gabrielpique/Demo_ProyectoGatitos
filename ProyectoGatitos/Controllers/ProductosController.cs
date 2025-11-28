using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using ProyectoGatitos.Models.AdopcionDTOs;
using ProyectoGatitos.Services;
using System.Security.Claims;

namespace ProyectoGatitos.Controllers
{
    public class ProductosController : Controller
    {
        public readonly TiendaService _tiendaService;

        public ProductosController(TiendaService tiendaService)
        {
            _tiendaService = tiendaService;
        }

        [HttpGet("tienda")]
        public async Task<IActionResult> Index()
        {
            var productos = await _tiendaService.GetAllProductos();
            return View(productos);
        }

        [HttpGet("producto/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var producto = await _tiendaService.GetProductoById(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductoDTO productoDto)
        {
            if (ModelState.IsValid)
            {
                await _tiendaService.RegistrarProducto(productoDto);
                return RedirectToAction("Index");
            }
            return View(productoDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _tiendaService.GetProductoById(id);

            if (producto == null)
            {
                return NotFound();
            }

            var editarDto = new EditarProductoDTO
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                Descripcion = producto.Descripcion
            };

            return View(editarDto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditarProducto(EditarProductoDTO editarProductoDTO)
        {
            if (ModelState.IsValid)
            {
                await _tiendaService.EditarProducto(editarProductoDTO);
                return RedirectToAction("Details", new { id = editarProductoDTO.Id });
            }
            return View("Edit", editarProductoDTO);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _tiendaService.GetProductoById(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var producto = await _tiendaService.GetProductoById(id);
                if (producto == null)
                {
                    return Json(new { success = false, message = "Producto no encontrada" });
                }


                var deleted = await _tiendaService.DeleteProducto(id);
                if (!deleted)
                {
                    return Json(new { success = false, message = "Error al eliminar" });
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CambiarEstadoStock(int id)
        {
            try
            {
                var producto = await _tiendaService.GetProductoById(id);
                if (producto == null)
                {
                    return Json(new { success = false, message = "Producto no encontrado" });
                }


                var cambiado = await _tiendaService.CambiarEstadoStock(id);
                if (!cambiado)
                {
                    return Json(new { success = false, message = "Error al cambiar estado del stock." });
                }

                return RedirectToAction("Details", new { id = producto.Id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
