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

//���U���� Login ���A�Ϊ� Session �A��
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

//���U Action Filter �L�o���A��
builder.Services.AddScoped<LoginStatusFilter>();

//���U IConfiguration�A�o��{���X�|�۰ʸ��J `appsettings.json`
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

//�ҥ� Session �A��
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
