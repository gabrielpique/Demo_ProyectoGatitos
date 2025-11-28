using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoGatitos.Models;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    // GET: /Account/Login
    [HttpGet("admin")]
    public IActionResult Login()
    {
        return View();
    }

    // POST: /Account/Login
    [HttpPost("admin")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Intento de inicio de sesión inválido.");
        }
        return View(model);
    }

    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Nombre = model.Nombre, Apellido = model.Apellido };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Asignar rol "User" por defecto
                await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    // POST: /Account/Logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult MakeAdmin()
    {
        return View();
    }
    // Acción para asignar rol Admin (protegida para admins)
    [Authorize(Roles = "Admin")] 
    [HttpPost]
    public async Task<IActionResult> MakeAdmin(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            TempData["Error"] = "Usuario no encontrado.";
            return RedirectToAction("Index", "Home");
        }

        // Verificar si ya tiene el rol
        if (!await _userManager.IsInRoleAsync(user, "Admin"))
        {
            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                TempData["Message"] = $"El usuario {email} ahora es Admin.";
            }
            else
            {
                TempData["Error"] = "Error al asignar el rol Admin.";
            }
        }
        else
        {
            TempData["Message"] = "El usuario ya es Admin.";
        }

        return RedirectToAction("Index", "Home");
    }
}