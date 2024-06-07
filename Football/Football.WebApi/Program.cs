using Autofac;
using Autofac.Extensions.DependencyInjection;
using Football.Service;
using AutoMapper;
using Football.Repository;
using Football.Service.Common;
using Football.Repository.Common;
using Football.WebApi;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder
        .RegisterType<FootballService>()
        .As<IFootballService>()
        .InstancePerDependency();

    containerBuilder
        .RegisterType<FootballRepository>()
        .As<IFootballRepository>()
        .InstancePerDependency();

    containerBuilder.RegisterInstance(mapper).As<IMapper>().SingleInstance();
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
