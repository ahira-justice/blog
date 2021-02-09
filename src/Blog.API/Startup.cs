using System;
using System.Reflection;
using AutoMapper;
using Blog.Application.Auth;
using Blog.Application.Mapper;
using Blog.Application.Repositories.AuthRepo;
using Blog.Application.Repositories.UserRepo;
using Blog.Application.Services.Auth;
using Blog.Application.Services.Users;
using Blog.Application.Settings;
using Blog.API.Auth.Token;
using Blog.API.Filters;
using Blog.API.Validators.Auth;
using Blog.Persistence.Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

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
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), errorCodesToAdd : null);
                });
            });

            // automapper
            services.AddAutoMapper(typeof(BlogMappings));

            // swagger
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

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
            services.AddScoped<IUsersService, UsersService>();
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
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
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
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
