using MyProject.Filters;
using MyProject.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<MyProjectContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MyProjectConnection")));

builder.Services.AddControllersWithViews(options => {
    options.Filters.Add<LogFilter>();
});

//註冊紀錄 Login 狀態用的 Session 服務
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

//註冊 Action Filter 過濾器服務
builder.Services.AddScoped<LoginStatusFilter>();

//註冊 IConfiguration，這行程式碼會自動載入 `appsettings.json`
var configuration = builder.Configuration;

builder.Services.AddSingleton<IConfiguration>(configuration);

//----------------------------------------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

//啟用 Session 服務
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
