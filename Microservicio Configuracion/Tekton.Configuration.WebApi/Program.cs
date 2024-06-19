using AutoMapper;
using Tekton.Configuration.Application;
using Tekton.Configuration.Application.Mappings;
using Tekton.Configuration.Infraestructure;
using Tekton.Service.Common.Extensiones;
using MediatR;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Tekton.Configuration.Application.Wrappers;
using Tekton.RedisCaching;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0.0",
        Title = builder.Configuration["swagger:title"],
        Description = builder.Configuration["swagger:description"],
        Contact = new OpenApiContact()
    });
    c.CustomSchemaIds(type => type.Name);
    c.AddSecurityDefinition("default", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.OAuth2,
        OpenIdConnectUrl = new Uri("https://tekton.com.pe:8243/authorize"),
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddApplication();


builder.Services.AddInfrastructure(builder.Configuration);

builder.AddCommonConfiguration(builder.Configuration);
builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutomapperProfile(provider.CreateScope().ServiceProvider.GetService<IRedisCacheService>()));
}).CreateMapper());


var app = builder.Build();
app.AddCommonConfiguration();
app.MapControllers();
app.Run();