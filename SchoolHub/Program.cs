using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolHub.Common.Data;
using SchoolHub.Common.Models.Usuarios;
using SchoolHub.Common.Repositories.Implementation;
using SchoolHub.Common.Repositories.Interface;
using SchoolHub.Mvc.Services.Implementation;
using SchoolHub.Mvc.Services.Interface;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ITurmaRepository, TurmaRepository>();
builder.Services.AddScoped<ITennantRepository, TennantRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<ISeedService, SeedService>();
builder.Services.AddScoped<IUploadFotoService, UploadFotoService>();

builder.Services.AddSingleton(RT.Comb.Provider.Sql);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<Usuario, Funcao>()
           .AddEntityFrameworkStores<AppDbContext>()
           .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);

    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LogoutPath = "/Account/Logout";
    options.SlidingExpiration = true;
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
app.UseAuthentication();
app.UseAuthorization();

using (var scope = app.Services.CreateScope())
{
    var seedUserService = scope.ServiceProvider.GetRequiredService<ISeedService>();
    seedUserService.Seed();
}

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
       name: "default",
       pattern: "{controller=Home}/{action=Index}/{id?}"
   );
});

app.Run();
