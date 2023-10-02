
using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.Validator;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Movie_API.Middleware;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core_Proje_Api
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
            var tokenSettings = Configuration.GetSection("TokenSettings");
            var secretKey = tokenSettings.GetValue<string>("SecretKey");
            var validIssuer = tokenSettings.GetValue<string>("ValidIssuer");
            var validAudience = tokenSettings.GetValue<string>("ValidAudience");

            // JWT doğrulama ayarlarını yapılandır
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = validIssuer,
                    ValidAudience = validAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddDbContext<Context>();

            services.AddScoped<IUserDal, EfUserDal>();
            services.AddScoped<IUserService, UserManager>();

            services.AddScoped<IMovieDal, EfMovieDal>();
            services.AddScoped<IMovieService, MovieManager>();

            services.AddScoped<IWatchedDal, EfWatchedDal>();
            services.AddScoped<IWatchedService, WatchedManager>();


            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddControllers();




            services.AddScoped<MovieValidator>();
          

            services.AddScoped<UserValidator>();


            //services.AddSingleton<IWatchedService, WatchedManager>();
            //services.AddTransient<IWatchedService, WatchedManager>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie API"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseMiddleware<UserIdMiddleware>();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}