using FA22.P02.Web.Features;
using Microsoft.AspNetCore.Builder;
using System.Reflection.Metadata.Ecma335;
using static FA22.P02.Web.Features.Products;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/get-all-products", () =>
{
    return Products;
});

app.MapGet("/api/products/{id}", (int id) =>
{
    return Products.Where(p => p.Id == id).FirstOrDefault();
})
.WithName("GET");

app.MapPost("/api/create-product", (ProductDto product) =>
{
    if (!Products.Where(p => p.Id == product.Id).Any())
    {
        Products.Add(product);
    }
});

app.Run();

//see: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0
// Hi 383 - this is added so we can test our web project automatically. More on that later
public partial class Program 
{
    public static List<ProductDto> Products = new List<ProductDto>
    {
        new ProductDto
        {
            Id = 1,
            Name = "Controller",
            Description = "red semi-used controller for xbox",
            Price = 24.99m,
        },

        new ProductDto
        {
            Id = 2,
            Name = "Xbox Series X",
            Description = "newest gen xbox, unopened",
            Price = 499.99m,
        },

        new ProductDto
        {
            Id = 3,
            Name = "Halo 5",
            Description = "Disk is in good condition, no noticeable damage",
            Price = 14.99m,
        },
    };
}
