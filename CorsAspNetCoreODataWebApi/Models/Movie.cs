using System.ComponentModel.DataAnnotations;

namespace CorsAspNetCoreODataWebApi.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Classification { get; set; }
        public int RunningTime { get; set; }
    }
}
