using Forge.Persistence;
using Forge.Services.Products;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddDbContext<ForgeDbContext>(options => 
    options.UseSqlite("Data Source=Forge.db")
    );
    // Add services to the container.

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    //builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();
}


var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    //}

    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();

    //app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

