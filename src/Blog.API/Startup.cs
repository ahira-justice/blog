using System;
using System.Reflection;
using AutoMapper;
using Blog.Application.Auth;
using Blog.Application.Mapper;
using Blog.Application.Repositories.AuthRepo;
using Blog.Application.Repositories.UserRepo;
using Blog.Application.Services.Auth;
using Blog.Application.Services.UserProfile;
using Blog.Application.Settings;
using Blog.API.Auth.Token;
using Blog.API.Filters;
using Blog.API.Validators.Auth;
using Blog.Persistence.Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using System.IO;

namespace Blog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // database
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), errorCodesToAdd: null);
                });
            });

            // automapper
            services.AddAutoMapper(typeof(BlogMappings));

            // swagger
            services.AddSwaggerGen(options =>
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

                options.SwaggerDoc("BlogOpenAPISpec", new OpenApiInfo
                {
                    Title = "Blog API",
                    Version = "1"
                });

                options.IncludeXmlComments(xmlCommentsFilePath);

                options.AddSecurityDefinition(bearerScheme.Reference.Id, bearerScheme);

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        bearerScheme, new string[] { }
                    }
                });

            });

            // controllers
            services.AddControllers(options =>
                {
                    options.Filters.Add(typeof(CustomExceptionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<RegisterDtoValidator>())
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });

            // cors
            services.AddCors();

            // repositories
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ITokenHandlerService, TokenHandlerService>();
            services.AddHttpContextAccessor();

            // filters
            services.AddScoped<LogUserActivityFilter>();

            // authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CustomAuthSchemes.BearerAuthScheme;
                options.DefaultChallengeScheme = CustomAuthSchemes.BearerAuthScheme;
            }).AddTokenAuthentication(CustomAuthSchemes.BearerAuthScheme, "Bearer Authentication Scheme", options => { });

            // settings
            services.Configure<ApplicationSettings>(Configuration.GetSection(nameof(ApplicationSettings)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/BlogOpenAPISpec/swagger.json", "Blog API");
                options.RoutePrefix = "api/blog";
            });
            app.UseRouting();
            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
