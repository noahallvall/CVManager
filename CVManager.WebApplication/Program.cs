using CVManager.DAL.Context;
using CVManager.DAL.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddIdentityApiEndpoints<User>()
                .AddEntityFrameworkStores<CVContext>();

var connString = builder.Configuration.GetConnectionString("CVContext");
builder.Services.AddSqlServer<CVContext>(connString);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.MapGroup("/identity")
   .MapIdentityApi<User>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
