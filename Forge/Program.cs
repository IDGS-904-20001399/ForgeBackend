using System.Data;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Forge.Services.Customers;
using Forge.Services.Dashboard;
using Forge.Services.Login;
using Forge.Services.Orders;
using Forge.Services.Products;
using Forge.Services.Suppliers;
using Forge.Services.Supplies;
using Forge.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<ISupplyService, SupplyService>();
    builder.Services.AddScoped<ILoginService, LoginService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ISupplierService, SupplierService>();
    builder.Services.AddScoped<ICustomerService, CustomerService>();
    builder.Services.AddScoped<IOrdersService, OrderService>();
    builder.Services.AddScoped<IDashboardService, DashboardService>();
    builder.Services.AddTransient<MySqlConnection>((sp) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            return new MySqlConnection(connectionString);
        });

    builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("admin"));
    options.AddPolicy("Customer", policy => policy.RequireRole("customer"));
    options.AddPolicy("Logged", policy => policy.RequireRole("admin", "customer",  "seller", "stocker"));
    options.AddPolicy("Orders", policy => policy.RequireRole("admin",   "seller"));
    options.AddPolicy("Stocker", policy => policy.RequireRole("admin",   "stocker"));
});
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })

    .AddJwtBearer(options =>
    {
        string Issuer = builder.Configuration.GetValue<string>("JWT:Issuer") ?? "";
        string Audience = builder.Configuration.GetValue<string>("JWT:Audience") ?? "";
        string secretKey = builder.Configuration.GetValue<string>("JWT:Key") ?? "";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
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
    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    app.UseCors();

    app.Run();
}

