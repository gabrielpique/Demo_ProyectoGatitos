using Microsoft.AspNetCore.Mvc;
using ProyectoGatitos.Services;
using ProyectoGatitos.Models;
using ProyectoGatitos.Models.AdopcionDTOs;
using ProyectoGatitos.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

// Por el momento no se utiliza esta funcionalidad
namespace ProyectoGatitos.Controllers
{
    public class AnimalesPerdidosController : Controller
    {
        private readonly AnimalPerdidoService _service;

        public AnimalesPerdidosController(AnimalPerdidoService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var animales = await _service.GetAllAnimalesPerdidos();
            return View(animales);
        }

        public async Task<IActionResult> Details(int id)
        {
            var animal = await _service.GetAnimalPerdidoById(id);
            if (animal == null)
            {
                return NotFound();
            }
            return View(animal);
        }

        // GET: /AnimalesPerdidos/CreatePerdido
        public IActionResult CreatePerdido()
        {
            return View(new AnimalPerdidoDTO { TipoPerdida = TipoPerdida.BuscadoPorDuenio });
        }

        // GET: /AnimalesPerdidos/CreateEncontrado
        public IActionResult CreateEncontrado()
        {
            return View("CreatePerdido", new AnimalPerdidoDTO { TipoPerdida = TipoPerdida.EncontradoSinDuenio });
        }

        // POST: /AnimalesPerdidos/CreatePerdido
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePerdido(AnimalPerdidoDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _service.CreateAnimalPerdido(dto);
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var animal = await _service.GetAnimalPerdidoById(id);
            if (animal == null)
            {
                return NotFound();
            }
            var dto = new AnimalPerdidoDTO
            {
                FechaEnQueSePerdio = animal.FechaEnQueSePerdio,
                FotosDelAnimal = animal.FotosDelAnimal,
                Mensaje = animal.Mensaje,
                Ubicacion = animal.Ubicacion,
                Recuperado = animal.Recuperado,
                NumeroContacto = animal.NumeroContacto,
                TipoPerdida = animal.TipoPerdida
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AnimalPerdidoDTO dto)
        {
            if (ModelState.IsValid)
            {
                var updated = await _service.ActualizarAnimalPerdido(dto, id);
                if (updated == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var animal = await _service.GetAnimalPerdidoById(id);
            if (animal == null)
            {
                return NotFound();
            }
            return View(animal);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var animal = await _service.GetAnimalPerdidoById(id);
                if (animal == null)
                {
                    return Json(new { success = false, message = "No encontrado" });
                }

                // Verificar permisos
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (animal.UsuarioPublicadorId != userId && !User.IsInRole("Admin"))
                {
                    return Json(new { success = false, message = "No tienes permisos" });
                }

                var deleted = await _service.DeleteAnimalPerdido(id);
                if (!deleted)
                {
                    return Json(new { success = false, message = "Error al eliminar" });
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> YaSeEncontro(int id)
        {

            bool resultado = await _service.YaSeEncontro(id);

            if (!resultado)
            {
                return NotFound("No se encontró o no se pudo confirmar.");
            }

            TempData["Mensaje"] = "Marcado como resuelto.";
            return RedirectToAction(nameof(Index));
        }
    }
}