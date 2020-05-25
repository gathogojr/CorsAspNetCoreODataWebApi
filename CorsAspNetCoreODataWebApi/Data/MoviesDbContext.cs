using CorsAspNetCoreODataWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace CorsAspNetCoreODataWebApi.Data
{
    public class MoviesDbContext : DbContext
    {
        public MoviesDbContext(DbContextOptions<MoviesDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
    }
}
