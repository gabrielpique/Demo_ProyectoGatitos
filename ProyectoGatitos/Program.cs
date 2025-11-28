using Microsoft.EntityFrameworkCore;
using ProyectoGatitos.Data;
using ProyectoGatitos.Services;
using ProyectoGatitos.Repositories;
using ProyectoGatitos.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DbContextProyectoGatitos>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<DbContextProyectoGatitos>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(6);
    options.SlidingExpiration = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IEntityBaseRepository<AnimalPerdido>, EntityBaseRepository<AnimalPerdido>>();
builder.Services.AddScoped<IEntityBaseRepository<Adopcion>, EntityBaseRepository<Adopcion>>();
builder.Services.AddScoped<IEntityBaseRepository<Formulario>, EntityBaseRepository<Formulario>>();
builder.Services.AddScoped<IEntityBaseRepository<Producto>, EntityBaseRepository<Producto>>();


builder.Services.AddScoped<AnimalPerdidoService>();
builder.Services.AddScoped<AdopcionesService>();
builder.Services.AddScoped<TiendaService>();




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Inicializar roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string[] roleNames = { "Admin", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}

app.Run();
