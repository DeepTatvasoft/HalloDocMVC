using Data.DataContext;
using DataAccess.ServiceRepository;
using DataAccess.ServiceRepository.IServiceRepository;
using HalloDoc.Hubs;
using Services.Contracts;
using Services.Implementation;
using Services.Implementationy;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddTransient<IFormSubmit, FormSubmit>();
builder.Services.AddTransient<IHomeFunction, HomeFunction>();
builder.Services.AddTransient<IDashboard, Dashboard>();
builder.Services.AddTransient<IAdminFunction, AdminFunction>();
builder.Services.AddTransient<IAuthorization, Authorization>();
builder.Services.AddTransient<IPhysicianFunction, PhysicianFunction>();
builder.Services.AddTransient<IChatFunction, ChatFunction>();
builder.Services.AddTransient<IJwtRepository, JwtRepo>();
builder.Services.AddSignalR();

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
app.UseSession();
app.UseRouting();
app.UseAuthorization();
app.MapHub<ChatHub>("/chatHub");
app.MapControllerRoute(
    name: "default",
    //pattern: "{controller=Home}/{action=patientsite}/{id?}");
pattern: "{controller=admin}/{action=adminlogin}/{id?}");

app.Run();
