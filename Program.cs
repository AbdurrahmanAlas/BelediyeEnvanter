using BelediyeEnvanter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connection = builder.Configuration.GetConnectionString("Default") ?? "Data Source=belediye.db";
var authConnection = builder.Configuration["DatabaseProvider"] == "SqlServer" ? (builder.Configuration.GetConnectionString("AuthSqlServer") ?? "") : (builder.Configuration.GetConnectionString("Auth") ?? "Data Source=auth.db");
if (builder.Configuration["DatabaseProvider"] == "SqlServer") builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connection));
else builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlite(connection));
if (builder.Configuration["DatabaseProvider"] == "SqlServer") builder.Services.AddDbContext<AuthDbContext>(o => o.UseSqlServer(authConnection));
else builder.Services.AddDbContext<AuthDbContext>(o => o.UseSqlite(authConnection));
builder.Services.AddIdentity<IdentityUser, IdentityRole>(o => { o.Password.RequiredLength = 8; o.Password.RequireNonAlphanumeric = false; o.Password.RequireUppercase = false; o.Password.RequireLowercase = false; o.Password.RequireDigit = false; }).AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthorization(o => o.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

var app = builder.Build();

using (var scope = app.Services.CreateScope()) { var db=scope.ServiceProvider.GetRequiredService<AppDbContext>(); SeedData.Initialize(db); scope.ServiceProvider.GetRequiredService<AuthDbContext>().Database.EnsureCreated(); await IdentitySeed.InitializeAsync(scope.ServiceProvider); }

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
