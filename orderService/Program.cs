using MassTransit;
using Microsoft.EntityFrameworkCore;
using orderService;
using orderService.Db;
using sharedModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMassTransit(config =>
{
  config.AddConsumer<CatalogResponseConsumer>();

  config.UsingRabbitMq((ctx, cfg) =>
  {
    cfg.Host("rabbitmq://localhost", h =>
    {
      h.Username("guest");
      h.Password("guest");
    });

    cfg.ReceiveEndpoint(EventBusConstants.CatalogResponseQueue, c =>
    {
      c.ConfigureConsumer<CatalogResponseConsumer>(ctx);
    });
  });
});

builder.Services.AddDbContext<OrderingDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
