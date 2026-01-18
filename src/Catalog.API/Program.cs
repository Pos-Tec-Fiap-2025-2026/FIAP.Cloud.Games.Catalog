using Catalog.Application.Extensions;
using Catalog.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataBaseContext(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddBus(builder.Configuration); 
builder.Services.AddRepository();
builder.Services.AddGameServices();
builder.Services.AddJwtAuthentication(builder);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();