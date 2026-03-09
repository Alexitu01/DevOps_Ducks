using ITUMiniTwit.Infrastructure.ITUMiniTwit.Service;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Repositories;
using ITUMiniTwit.Infrastructure.ITUMiniTwit.Moniter;
using ITUMiniTwit.Infrastructure;
using ITUMiniTwit.Web;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ITUMiniTwit.Core.Models;
using ITUMiniTwit.Web.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddScoped<ICheepService, CheepService>();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddSingleton<ILoginMetrics, LoginMetrics>();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));

builder.Services.AddDbContext<ITUMiniTwitDBContext>(options => options.UseMySql(connectionString, serverVersion));

builder.Services.AddDefaultIdentity<Author>(options => {
    options.SignIn.RequireConfirmedAccount = false;

    // Relax password policy for simulator
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -_.'";}
    )
    .AddEntityFrameworkStores<ITUMiniTwitDBContext>();
builder.Services
    .AddAuthentication(options =>
    {
        options.RequireAuthenticatedSignIn = true;
    });
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(60);
});

builder.Services.UseHttpClientMetrics();

var app = builder.Build();

// Create a disposable service scope
using (var scope = app.Services.CreateScope())
{
    // From the scope, get an instance of our database context.
    // Through the `using` keyword, we make sure to dispose it after we are done.
    using var context = scope.ServiceProvider.GetRequiredService<ITUMiniTwitDBContext>();

    // Execute the migration from code.
    context.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseHttpMetrics();
app.MapMetrics();

app.UseAuthentication();
app.UseAuthorization();

//collect and display http metrics
app.UseHttpMetrics();


app.MapRazorPages();
app.MapControllers();
app.MapMetrics();

app.Run();

public partial class Program { } // For integration tests