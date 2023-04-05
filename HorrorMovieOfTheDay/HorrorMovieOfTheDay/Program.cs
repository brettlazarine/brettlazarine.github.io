using HorrorMovieOfTheDay;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

static void Main(string[] args)
{
    var key = "2e5aaec53df627a3c2f12b971432f2ef";
    var movieOfTheDay = HorrorMovieAPI.GetMovieInfo(key);
    var movieVideo = HorrorMovieAPI.GetVideo(key, movieOfTheDay); //needs adjustment!
    //HorrorMovieAPI.GetVideo(key, movieOfTheDay);
    Console.WriteLine($"Movie id: {movieOfTheDay.id},\n" +
        $"Movie Title: {movieOfTheDay.title},\n" +
        $"Movie Release Date {movieOfTheDay.release_date},\n" +
        $"Movie Poster Path: {movieOfTheDay.poster_path},\n" +
        $"Movie Video Path: {movieVideo}");
}