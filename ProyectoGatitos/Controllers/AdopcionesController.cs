using Microsoft.AspNetCore.Mvc;
using ProyectoGatitos.Services;
using ProyectoGatitos.Models;
using ProyectoGatitos.Models.AdopcionDTOs;
using ProyectoGatitos.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ProyectoGatitos.Controllers
{
    public class AdopcionesController : Controller
    {
        private readonly AdopcionesService _adopcionesService;

        public AdopcionesController(AdopcionesService adopcionesService)
        {
            _adopcionesService = adopcionesService;
        }

        [HttpGet("adoptar")]
        public async Task<IActionResult> Index()
        {
            var adopciones = await _adopcionesService.GetAllAdopciones();
            return View(adopciones);
        }


        [HttpGet("requisitos")]
        public IActionResult RequisitosParaAdoptar()
        {
            return View();
        }

        [HttpGet("adoptar-gato")]
        public async Task<IActionResult> AdopcionesPendientes(int page = 1)
        {
            //int pageSize = 9;
            //var adopcionesPendientes = await _adopcionesService.GetAdopcionesPendientes(page, pageSize);
            
            //return View("Index", adopcionesPendientes);
            int pageSize = 9;
            var totalAdopciones = await _adopcionesService.GetTotalAdopcionesPendientes();
            var adopciones = await _adopcionesService.GetAdopcionesPendientes(page, pageSize);
            int cantidadGatosEnAdopcion = await _adopcionesService.GetCantidadGatosPendientesDeAdopcion();

            var viewModel = new AdopcionVM
            {
                Adopciones = adopciones,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalAdopciones / (double)pageSize),
                NombreAccion = "AdopcionesPendientes",
                CantidadTotalGatos = cantidadGatosEnAdopcion
            };

            ViewBag.Vista = "Pendientes";
            return View("Index", viewModel);
        }

        [HttpGet("adoptar-refugios-externos")]
        public async Task<IActionResult> AdopcionesPendientesRefugios(int page = 1)
        {
            int pageSize = 9;
            var total = await _adopcionesService.GetTotalAdopcionesRefugioPendientes();
            var adopciones = await _adopcionesService.GetAdopcionesPendientesRefugiosExternos(page, pageSize);
            int cantidadGatos = await _adopcionesService.GetCantidadGatosPendientesDeAdopcionRefugio();

            var viewModel = new AdopcionVM
            {
                Adopciones = adopciones,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(total / (double)pageSize),
                NombreAccion = "AdopcionesPendientesRefugios",
                CantidadTotalGatos = cantidadGatos
            };

            ViewBag.EsRefugio = true;
            ViewBag.Vista = "Pendientes";
            return View("Index", viewModel);
        }


        [HttpGet("gatos-adoptados")]
        public async Task<IActionResult> AdopcionesRealizadas(int page = 1)
        {
            //int pageSize = 9;
            //var adopciones = await _adopcionesService.GetAdopcionesRealizadas(page, pageSize);
            
            //return View("Index", adopciones);
            int pageSize = 9;
            var totalAdopciones = await _adopcionesService.GetTotalAdopcionesRealizadas();
            var adopciones = await _adopcionesService.GetAdopcionesRealizadas(page, pageSize);
            int cantidadGatosAdoptados = await _adopcionesService.GetCantidadGatosAdoptados();

            var viewModel = new AdopcionVM
            {
                Adopciones = adopciones,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalAdopciones / (double)pageSize),
                NombreAccion = "AdopcionesRealizadas",
                CantidadTotalGatos = cantidadGatosAdoptados
            };
            ViewBag.Vista = "Adoptados";
            return View("Index", viewModel);
        }

        [HttpGet("gatos-adoptados-refugios-externos")]
        public async Task<IActionResult> AdopcionesRealizadasRefugiosExternos(int page = 1)
        {
            //int pageSize = 9;
            //var adopciones = await _adopcionesService.GetAdopcionesRealizadas(page, pageSize);

            //return View("Index", adopciones);
            int pageSize = 9;
            var totalAdopciones = await _adopcionesService.GetTotalAdopcionesRealizadasRefugiosExternos();
            var adopciones = await _adopcionesService.GetAdopcionesRealizadasRefugiosExternos(page, pageSize);
            int cantidadGatosAdoptados = await _adopcionesService.GetCantidadGatosAdoptadosRefugiosExternos();

            var viewModel = new AdopcionVM
            {
                Adopciones = adopciones,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalAdopciones / (double)pageSize),
                NombreAccion = "AdopcionesRealizadas",
                CantidadTotalGatos = cantidadGatosAdoptados
            };
            ViewBag.EsRefugio = true;
            ViewBag.Vista = "Adoptados";
            return View("Index", viewModel);
        }

        [HttpGet("adopcion/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var adopcion = await _adopcionesService.GetAdopcionById(id);
            if (adopcion == null)
            {
                return NotFound();
            }
            return View(adopcion);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var dto = new AdopcionDTO
            {
                CantidadGatos = 1 // valor por defecto
            };
            return View(dto);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult CrearDesdeRefugio()
        {
            var model = new AdopcionDTO
            {
                EsDeRefugio = true
            };

            return View("Create", model); // Reutiliza la vista base
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(AdopcionDTO dto)
        {
            if (ModelState.IsValid)
            {
                await _adopcionesService.CreateAdopcion(dto);
                if (dto.EsDeRefugio)
                {
                    return RedirectToAction("AdopcionesPendientesRefugios");
                }
                return RedirectToAction("AdopcionesPendientes");
            }
            return View(dto);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, string origen)
        {
            var adopcion = await _adopcionesService.GetAdopcionById(id);
            if (adopcion == null)
            {
                return NotFound();
            }
            var dto = new AdopcionDTO
            {
                Id = adopcion.Id,
                Nombre = adopcion.Nombre,
                Edad = adopcion.Edad,
                Sexo = adopcion.Sexo,
                NombreRefugio = adopcion.NombreRefugio,
                ContactoRefugio = adopcion.ContactoRefugio,
                EsDeRefugio = adopcion.EsDeRefugio,
                FechaDeAdopcion = adopcion.FechaDeAdopcion,
                CantidadGatos = adopcion.CantidadGatos,
                Descripcion = adopcion.Descripcion,
                PersonaQueAdopto = adopcion.PersonaQueAdopto,
                Adoptado = adopcion.Adoptado
            };
            ViewBag.Vista = origen;
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, AdopcionDTO dto, string origen) // origen es para ver de que vista proviene, pendientes o adoptados
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _adopcionesService.ActualizarAdopcion(dto);
                    if (origen == "Pendientes")
                    {
                        return RedirectToAction(dto.EsDeRefugio
                            ? "AdopcionesPendientesRefugios"
                            : "AdopcionesPendientes");
                    }
                    else
                    {
                        return RedirectToAction(dto.EsDeRefugio
                            ? "AdopcionesRealizadasRefugiosExternos"
                            : "AdopcionesRealizadas");
                    }

                }
                catch (KeyNotFoundException)
                {
                    return NotFound();
                }
            }
            return View(dto);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, string origen)
        {
            var adopcion = await _adopcionesService.GetAdopcionById(id);
            if (adopcion == null)
            {
                return NotFound();
            }
            ViewBag.Origen = origen;
            return View(adopcion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id, string origen)
        {
            try
            {
                var adopcion = await _adopcionesService.GetAdopcionById(id);
                if (adopcion == null)
                {
                    return Json(new { success = false, message = "Adopción no encontrada" });
                }

                // Verificar permisos
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (adopcion.UsuarioPublicadorId != userId && !User.IsInRole("Admin"))
                {
                    return Json(new { success = false, message = "No tienes permisos" });
                }

                var deleted = await _adopcionesService.DeleteAdopcion(id);
                if (!deleted)
                {
                    return Json(new { success = false, message = "Error al eliminar" });
                }

                if(origen == "Pendientes")
                {
                    return RedirectToAction("AdopcionesPendientes");
                }
                else
                {
                    return RedirectToAction("AdopcionesRealizadas");
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarAdopcion(int id, string personaQueAdopto)
        {
            if (string.IsNullOrWhiteSpace(personaQueAdopto))
            {
                ModelState.AddModelError("", "El nombre del adoptante es obligatorio.");
                return RedirectToAction(nameof(Edit), new { id });
            }

            bool resultado = await _adopcionesService.ConfirmarAdopcion(id, personaQueAdopto);

            if (!resultado)
            {
                return NotFound("No se encontró la adopción o no se pudo confirmar.");
            }

            TempData["Mensaje"] = "Adopción confirmada exitosamente.";
            return Json(new { success = true, message = "Adopción confirmada" });
        }

        [HttpGet("formulario")]
        public IActionResult EnviarFormulario()
        {
            return View();
        }

        [HttpPost("formulario")]
        public async Task<IActionResult> EnviarFormulario(Formulario formulario)
        {

            if (ModelState.IsValid)
            {
                await _adopcionesService.EnviarFormulario(formulario);
                return RedirectToAction("Index", "Home");
            }
            return View(formulario);
        }

        [HttpGet("formularios")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllFormularios()
        {
            var formularios = await _adopcionesService.GetAllFormularios();
            return View(formularios);
        }

        [HttpGet("formulario/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFormulario(int id)
        {
            var formulario = await _adopcionesService.GetFormularioById(id);
            if(formulario == null)
            {
                return NotFound();
            }
            return View(formulario);
        }

        // formulario revisado
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MarcarComoRevisado(int id)
        {
            await _adopcionesService.MarcarComoRevisado(id);
            return Json(new { success = true, message = "Formulario revisado." });
        }

    }
}