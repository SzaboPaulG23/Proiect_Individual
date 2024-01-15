using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CatalogOnline2.Data;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CatalogOnline2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CatalogOnline2Context") ?? throw new InvalidOperationException("Connection string 'CatalogOnline2Context' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
