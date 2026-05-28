var builder = WebApplication.CreateBuilder(args);

// Agrega soporte para MVC (Controllers + Views)
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();

// Ruta por defecto: va directo al listado de materias
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Materias}/{action=Index}/{id?}");

app.Run();
