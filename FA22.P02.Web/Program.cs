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

app.MapGet("/api/products", () =>
{
    return Products;
})
.WithName("GetAll");

app.MapGet("/api/products/{id}", (int id) =>
{
    if (Products.Where(p => p.Id == id).Any())
    {
        var productToShow = Products.First(p => p.Id == id);

        return Results.Ok(productToShow);
    }
    else
    {
        return Results.NotFound();
    }
})
.WithName("GetById");

app.MapPost("/api/products", (ProductDto product) =>
{
    if (!Products.Where(p => p.Id == product.Id).Any() && product.Name != null && product.Name.Length < 120 
        && product.Description != null && product.Price != null && product.Price > 0)
    {
        product.Id = SetIdForProduct();
        Products.Add(product);

        return Results.Created($"http://localhost/api/products/{product.Id}", product);
    }
    else
    {
        return Results.BadRequest();
    }
})
.WithName("POST");

app.MapPut("/api/products/{id}", (int id, ProductDto editedProduct) =>
{
    if (Products.Where(p => p.Id == id).Any())
    {
        if (editedProduct.Id > 0 && editedProduct.Name != null && editedProduct.Name.Length <= 120
            && editedProduct.Description != null && editedProduct.Price != null && editedProduct.Price > 0)
        {
            var productToEdit = Products.FirstOrDefault(p => p.Id == id);
         
            productToEdit.Id = editedProduct.Id;
            productToEdit.Name = editedProduct.Name;
            productToEdit.Description = editedProduct.Description;
            productToEdit.Price = editedProduct.Price;

            return Results.Ok(editedProduct);
        }

        return Results.BadRequest();
    }
    else
    {
        return Results.NotFound();
    }
})
.WithName("PUT");

app.MapDelete("/api/products/{id}", (int id) =>
{

    if (Products.Where(p => p.Id == id).Any())
    {
        var productToDelete = Products.First(p => p.Id == id);
        Products.Remove(productToDelete);

        return Results.Ok();

    }
    else
    {
        return Results.NotFound();
    }
})
.WithName("DELETE");


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

    public static int SetIdForProduct()
    {
        int id = Products.Select(p => p.Id).ToList().Max() + 1;

        return id;
    }
}
