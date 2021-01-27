using System;
using System.Reflection;
using AutoMapper;
using Blog.Application.Mapper;
using Blog.Application.Repositories.AuthRepo;
using Blog.Application.Repositories.UserRepo;
using Blog.Application.Services.Auth;
using Blog.Application.Settings;
using Blog.API.Validators.Auth.Request;
using Blog.Persistence.Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            // controllers
            services.AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<RegisterDtoValidator>());

            // cors
            services.AddCors();

            // repositories
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // services
            services.AddScoped<IAuthService, AuthService>();

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
