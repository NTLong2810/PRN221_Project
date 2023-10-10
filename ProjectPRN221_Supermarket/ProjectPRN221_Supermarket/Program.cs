using Microsoft.EntityFrameworkCore;
using ProjectPRN221_Supermarket.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<SupermarketDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
