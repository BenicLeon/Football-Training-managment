using Autofac;
using Autofac.Extensions.DependencyInjection;
using Football.Service;
using Football.Repository;
using Football.Service.Common;
using Football.Repository.Common;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
