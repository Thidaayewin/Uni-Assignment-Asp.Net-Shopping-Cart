using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCart.Data;
using ShoppingCart.Middleware;
using ShoppingCart.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();
builder.Services.AddSingleton<Data>();
builder.Services.AddSingleton<AccountData>();
builder.Services.AddSingleton<CartData>();
builder.Services.AddSingleton<Session>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(15); //session ID will change after 15 mins of inactivity on website, user, if logged in, will have to relogin.
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<SessionCleanupMiddleware>(); //register dependency injection of custom middleware

var app = builder.Build();
app.UseSession();
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
app.UseMiddleware<SessionCleanupMiddleware>(); //cleanup SQL cart table associated with previous sessionID but no customerID, after a session has timed out
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "home",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "Account/Login");
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
	   name: "default",
	   pattern: "{controller=Home}/{action=Index}/{id?}");
	endpoints.MapControllerRoute(
        name: "login",
        pattern: "{controller=Account}/{action=Login}");
   
});

InitCache(app.Services);
void InitCache(IServiceProvider serviceProvider)
{
    var cache = serviceProvider.GetRequiredService<IMemoryCache>();
    var session = serviceProvider.GetRequiredService<ISessionStore>();
   
}

app.Run();
