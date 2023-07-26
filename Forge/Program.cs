using System.Data;
using Forge.Services.Products;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddTransient<MySqlConnection>((sp) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            return new MySqlConnection(connectionString);
        });
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

