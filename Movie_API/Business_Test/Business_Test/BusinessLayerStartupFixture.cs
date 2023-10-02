using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using BusinessLayer.Validator;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Microsoft.DependencyInjection;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace Business_Test.Business_Test
{
    public class BusinessLayerStartupFixture: TestBedFixture , IClassFixture<BusinessLayerStartupFixture>
    {

        protected override void AddServices(IServiceCollection services, IConfiguration? configuration)
        {
           
            services.AddTransient<IUserDal, EfUserDal>();
            services.AddTransient<IUserService, UserManager>();

            services.AddTransient<IMovieDal, EfMovieDal>();
            services.AddTransient<IMovieService, MovieManager>();

            services.AddTransient<IWatchedDal, EfWatchedDal>();
            services.AddTransient<IWatchedService, WatchedManager>();
            services.AddDbContext<Context>();

            services.AddScoped<MovieValidator>();

            services.AddScoped<UserValidator>();

        }

        protected override ValueTask DisposeAsyncCore() => new();

        protected override IEnumerable<TestAppSettings> GetTestAppSettings()
        {
            yield return new() { Filename = "appsettings.json", IsOptional = false };
        }


        
    }
}
