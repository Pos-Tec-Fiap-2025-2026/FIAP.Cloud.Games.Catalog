using Catalog.Infrastructure.Extensions;
using Catalog.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();

builder.Services.AddSingleton<InMemoryDatabase>(); 
builder.Services.AddBus(builder.Configuration); 

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