using EventBus.Messages.Common;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.API.Extensions;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// MassTransit-RabbitMQ Configuration
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>(); // end consumer
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]); // AMQP Advanced Message Queuing Protocol. it creates a service bus
        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        }); // subscribing basket checkout method
    });
});

// General Configuration
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<BasketCheckoutConsumer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

builder.Configuration.MigrateDatabase<OrderContext>(app.Services, (context, services) =>
{    
    OrderContextSeed.SeedAsync(context, app.Logger).Wait();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
