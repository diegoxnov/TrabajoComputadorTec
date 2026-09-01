using InventarioPC.Business.Servicios;
using InventarioPC.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<InventarioPcDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IArmadoRepository, ArmadoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IArmadoService, ArmadoService>();

const string CorsPolicy = "AngularFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(CorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
