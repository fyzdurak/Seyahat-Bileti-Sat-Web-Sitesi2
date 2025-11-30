using BiletSatis.Models;
using BiletSatisWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Veritabanı bağlantısı (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2️⃣ Identity (Kullanıcı ve Rol yönetimi)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3️⃣ MVC + Session + Cache + HttpContextAccessor
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache(); // <-- önce cache
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// 4️⃣ Role ve admin kullanıcı oluşturma
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await CreateRolesAndAdminUser(serviceProvider);
}

// 5️⃣ Middleware sırası (çok önemli)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//  ⚠️ Sıra önemlidir:
app.UseAuthentication();
app.UseAuthorization();
app.UseSession(); // authorization'dan sonra, endpoint'ten önce

// 6️⃣ Varsayılan rota
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


// ---------------------------
// 7️⃣ Admin kullanıcı oluşturma metodu
// ---------------------------
async Task CreateRolesAndAdminUser(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roleNames = { "Admin", "User" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var adminEmail = "admin@rota.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        var admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            Ad = "Admin",
            Soyad = "Kullanıcı",
            UserType = "Admin"
        };

        string adminPassword = "Admin123!";
        var createAdmin = await userManager.CreateAsync(admin, adminPassword);

        if (createAdmin.Succeeded)
        {
            await userManager.AddToRoleAsync(admin, "Admin");
        }
    }
}


