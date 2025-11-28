using Microsoft.AspNetCore.Mvc;
using ProyectoGatitos.Models;
using ProyectoGatitos.Services;
using System.Diagnostics;

namespace ProyectoGatitos.Controllers
{
    public class HomeController : Controller
    {
        private readonly AdopcionesService _adopcionesService;
        private readonly ILogger<HomeController> _logger;


        public HomeController(AdopcionesService adopcionesService, ILogger<HomeController> logger)
        {
            _adopcionesService = adopcionesService;
            _logger = logger;
        }


        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public async Task<IActionResult> Index()
        {
            var cantidadFormulariosSinRevisar = await _adopcionesService.GetCountFormulariosSinRevisar();
            return View(cantidadFormulariosSinRevisar);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet("donar")]
        public IActionResult Donar()
        {
            return View();
        }
        [HttpGet("presentacion")]
        public IActionResult Presentacion()
        {
            return View();
        }
        [HttpGet("transitar")]
        public IActionResult Transitar()
        {
            return View();
        }
        [HttpGet("refugios-externos")]
        public IActionResult RefugiosExternos()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
