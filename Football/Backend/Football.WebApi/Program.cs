using Autofac;
using Autofac.Extensions.DependencyInjection;
using Football.Service;
using AutoMapper;
using Football.Repository;
using Football.Service.Common;
using Football.Repository.Common;
using Football.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Football.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

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

    containerBuilder.Register(ctx =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        return new FootballRepository(connectionString);
    }).As<IFootballRepository>()
      .InstancePerDependency();

    containerBuilder
       .RegisterType<TrainingService>()
       .As<ITrainingService>()
       .InstancePerDependency();

    containerBuilder.Register(ctx =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        return new TrainingRepository(connectionString);
    }).As<ITrainingRepository>()
      .InstancePerDependency();

    containerBuilder.RegisterInstance(mapper).As<IMapper>().SingleInstance();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseMiddleware<AuthMiddleware>();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();
app.Run();
