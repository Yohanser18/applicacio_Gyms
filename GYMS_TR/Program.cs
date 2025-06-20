using GYMS_TR.Datos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Este es el servicio para poder conectarnos la base de datos y hacer las inyecciones corespondiente a la base de datos de sqlserver//
// convertir las entidades de nuestros modelos en tabls de la base de datos, atra ves del DbContext//
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Aqui estaremos agregandr el servicion para poder trabajar con lo que son las intefaces de usuario con scaffoid //
builder.Services.AddDefaultIdentity<IdentityUser>()
                                  .AddEntityFrameworkStores<ApplicationDbContext>();

//Aqui vamos a crear las sesiones para que padamos agregar al corro de compros//
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(Options =>
{
    Options.IdleTimeout = TimeSpan.FromSeconds(10);
    Options.Cookie.HttpOnly = true;
    Options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Aqui van las auteticacion//
app.UseAuthentication();

app.UseAuthorization();

app.UseSession(); // Le vamos a decir que use las seciones //

//Esto es para que se puedan ver las vistas de razor pages//
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
