using BookStoreApp.Models;
using BookStoreApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// builder.Services.AddHttpClient("MyHttpClient", client =>
// {
//     client.BaseAddress = new Uri("https://localhost:7266/api");
//     // Configure any other HttpClient settings here.
// });
builder.Services.AddTransient<ISongService, SongService>();
builder.Configuration.Bind("ApiSettings", new ApiSettings()); // ApiSettings is a class you'll create

// Register your services
builder.Services.AddHttpClient("MyHttpClient", client =>
{
    var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
    // Configure any other HttpClient settings here.
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Song}/{action=Index}/{id?}");

app.Run();
