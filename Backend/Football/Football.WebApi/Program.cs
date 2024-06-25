using Autofac;
using Autofac.Extensions.DependencyInjection;
using Football.Service;
using AutoMapper;
using Football.Repository;
using Football.Service.Common;
using Football.Repository.Common;
using Football.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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
        .SingleInstance();

    containerBuilder
        .RegisterType<FootballRepository>()
        .As<IFootballRepository>()
        .SingleInstance();

    containerBuilder.RegisterInstance(mapper).As<IMapper>().SingleInstance();
});

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
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


app.UseCors("AllowReactApp");

app.UseAuthorization();
app.MapControllers();
app.Run();
