using Autofac;
using Autofac.Extensions.DependencyInjection;
using Football.Service;
using Football.Repository;
using Football.Service.Common;
using Football.Repository.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use Autofac as the DI container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder
        .RegisterType<FootballService>()
        .As<IFootballService>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<FootballRepository>()
        .As<IFootballRepository>()
        .InstancePerLifetimeScope();
});

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
