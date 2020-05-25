using CorsAspNetCoreODataWebApi.Data;
using CorsAspNetCoreODataWebApi.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System.Linq;


namespace CorsAspNetCoreODataWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MoviesDbContext>(options => options.UseInMemoryDatabase(databaseName: "MoviesDb"));
            // Set up Cross-Origin Resource Sharing
            services.AddCors(
                options => options.AddPolicy(
                    Constants.AllowSpecificOriginsCorsPolicy,
                    builder => builder.WithOrigins("http://localhost:2200").AllowAnyHeader().AllowAnyMethod()));
            services.AddControllers();
            services.AddOData();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            // UseCors() must be placed after UseRouting and before UseAuthorization
            // Middleware order: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1#middleware-order
            app.UseCors();  // Policy applied using EnableCors("#Policy#") attribute on the controller action
            // app.UseCors(Constants.AllowSpecificOriginsCorsPolicy);

            app.UseEndpoints(builder =>
            {
                builder.MapControllers();
                builder.Select().Filter().Expand().Count().OrderBy().SkipToken().MaxTop(100);
                builder.MapODataRoute("odata", "odata", GetEdmModel());
            });

            AddSeedData(app);
        }

        private static void AddSeedData(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetRequiredService<MoviesDbContext>();
                if (!db.Movies.Any())
                {
                    db.Movies.Add(new Movie { Name = "Bombshell", Classification = "R", RunningTime = 108 });
                    db.Movies.Add(new Movie { Name = "Ad Astra", Classification = "PG-13", RunningTime = 124 });
                    db.Movies.Add(new Movie { Name = "Schindler's List", Classification = "R", RunningTime = 195 });
                    db.Movies.Add(new Movie { Name = "Godzilla: King of the Monsters", Classification = "PG-13", RunningTime = 132 });
                    db.Movies.Add(new Movie { Name = "Ford v Ferrari", Classification = "PG-13", RunningTime = 152 });
                    db.Movies.Add(new Movie { Name = "Mad Max: Fury Road", Classification = "R", RunningTime = 120 });
                    db.Movies.Add(new Movie { Name = "Star Wars: The Rise of Skywalker", Classification = "PG-13", RunningTime = 143 });
                    db.Movies.Add(new Movie { Name = "Doctor Strange", Classification = "PG-13", RunningTime = 115 });
                    db.Movies.Add(new Movie { Name = "Dolittle", Classification = "PG", RunningTime = 101 });
                    db.Movies.Add(new Movie { Name = "Knives Out", Classification = "PG-13", RunningTime = 130 });
                    db.Movies.Add(new Movie { Name = "The Equalizer", Classification = "R", RunningTime = 132 });

                    db.SaveChanges();
                }
            }
        }

        private IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Movie>("Movies");
            
            return builder.GetEdmModel();
        }
    }
}
