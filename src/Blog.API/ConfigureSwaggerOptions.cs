using System;
using System.IO;
using System.Reflection;
using Blog.Application.Auth;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Blog.API
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                var bearerScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = CustomAuthSchemes.BearerAuthScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                options.SwaggerDoc(
                    description.GroupName, new OpenApiInfo
                    {
                        Title = $"Blog API {description.ApiVersion}",
                            Version = description.ApiVersion.ToString()
                    }
                );
                options.IncludeXmlComments(xmlCommentsFilePath);
                options.AddSecurityDefinition(bearerScheme.Reference.Id, bearerScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        bearerScheme,
                        new string[] { }
                    }
                });
            }
        }
    }
}
