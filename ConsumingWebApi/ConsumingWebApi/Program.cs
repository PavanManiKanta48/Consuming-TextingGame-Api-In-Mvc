using Newtonsoft.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddNewtonsoftJson
    (options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson
    (options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
builder.Services.AddMvc()
.AddNewtonsoftJson(options =>
       options.SerializerSettings.ContractResolver =
          new CamelCasePropertyNamesContractResolver());
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//// Add services to the container.
//builder.Services.AddControllersWithViews();
////builder.Configuration.AddJsonFile("appsettings.json",optional:true);
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromSeconds(1800);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
//builder.Services.Configure<APIDetails>(builder.Configuration.GetSection("APIDetails"));
var app = builder.Build();

// Configure the HTTP request pipeline.

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Room}/{action=Index}/{id?}");

app.Run();
