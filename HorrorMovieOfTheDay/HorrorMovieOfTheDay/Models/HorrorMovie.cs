using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HorrorMovieOfTheDay.Models
{
    public class HorrorMovie
    {
        public string id { get; set; }
        public string overview { get; set; }
        public string release_date { get; set; }
        public string title { get; set; }
        public double vote_average { get; set; }
        public string poster_path { get; set; }
        public string video_path { get; set; }
    }
}
