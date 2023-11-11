using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Hubs;
using ProjectPRN221_Supermarket.Models;
using ProjectPRN221_Supermarket.Repository;
using ProjectPRN221_Supermarket.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddDbContext<SupermarketDBContext>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapRazorPages();
app.MapGet("/", context =>
{
	context.Response.Redirect("/Login");
	return Task.CompletedTask;
});
app.MapHub<HubServer>("/productHub");

app.Run();
