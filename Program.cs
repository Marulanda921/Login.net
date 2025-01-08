using ApiLoginFull.Services;

var builder = WebApplication.CreateBuilder(args);


//QUITAR CORS DE TODOS LOS ORIGENES
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()         // Permite solicitudes desde cualquier origen
              .AllowAnyMethod()         // Permite cualquier método HTTP (GET, POST, etc.)
              .AllowAnyHeader();        // Permite cualquier encabezado
    });
});

// Add services to the container.
// Agregar servicios a la colección
builder.Services.AddSingleton<UserCredentialsService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at
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

app.UseAuthorization();

app.MapControllers();

app.Run();
