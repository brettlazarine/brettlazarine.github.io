using Newtonsoft.Json;

namespace HorrorMovieOfTheDay
{
    public class HorrorMovieAPI
    {
        public static Result GetMovieInfo(string key)
        {
            var client = new HttpClient();

            int page;
            var movieOfDay = new Result();
            var date = DateTime.UtcNow.AddHours(-6).ToString("yyyy-MM-dd"); //format today's date to pull for a specific release date
            //Console.WriteLine(date[5..]); //print to screen the date used to specify the desired movie

            Root root = new Root(); //instantiate root to begin processing API, can use in for-loops this way
            var apiURL = $"https://api.themoviedb.org/3/discover/movie?api_key={key}&with_genres=27"; //specify horror genre 27
            var response = client.GetStringAsync(apiURL).Result;
            root = JsonConvert.DeserializeObject<Root>(response); //get initial base values

            movieOfDay.id = root.results[0].id; //initialize movieID with a guaranteed film should no release date match today's date
            movieOfDay.release_date = root.results[0].release_date;
            movieOfDay.vote_average = 0; //set to 0 for later comparison

            //Console.WriteLine(movieID); verify movieID is capturing a film ID value
            for (page = 1; page <= 100; page++)
            {//iterate through multiple pages to find the film with the desired release date
                response = client.GetStringAsync($"https://api.themoviedb.org/3/discover/movie?api_key={key}&with_genres=27&page={page}").Result;
                root = JsonConvert.DeserializeObject<Root>(response);
                foreach (var item in root.results)
                {
                    if (item.release_date == null || item.release_date == "") { continue; } //null/empty string check
                    if (item.release_date[5..] == date[5..] && item.vote_average > movieOfDay.vote_average) //ensure the film released *today* and has a higher voter score than the previous value
                    {
                        movieOfDay.id = item.id;
                        movieOfDay.release_date = item.release_date;
                        movieOfDay.vote_average = item.vote_average;
                        movieOfDay.title = item.title;
                        movieOfDay.overview = item.overview;
                        movieOfDay.poster_path = $"https://image.tmdb.org/t/p/w185{item.poster_path}";
                        movieOfDay.video = item.video;
                    }
                    //Console.WriteLine(item.release_date); //verify the loop is iterating through the pages
                }
            }
            //$"https://api.themoviedb.org/3/movie/{movieOfDay.id}/videos?api_key={key}"

            //Console.WriteLine($"{movieOfDay.id}, {movieOfDay.release_date}, {movieOfDay.vote_average}"); //verify info

            return movieOfDay;
        }

        public static string GetVideo(string key, Result movie)
        {
            var client = new HttpClient();

            var videoURL = $"https://api.themoviedb.org/3/movie/{movie.id}/videos?api_key={key}&language=en-US";

            string videoLink = "";

            var response = client.GetStringAsync(videoURL).Result;
            Root root = JsonConvert.DeserializeObject<Root>(response);

            foreach (var item in root.results)
            {
                if (item.site == "YouTube" && item.type == "Trailer")
                {
                    videoLink = $"https://www.youtube.com/watch?v={item.key}/html5";
                }
                else
                {
                    videoLink = "Not found.";
                }
            }
            return videoLink;
        }
    }
}
