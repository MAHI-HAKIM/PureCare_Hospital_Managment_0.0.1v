using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.Areas.Identity.Data;
using PureCareHub_HospitalCare.Models.Service;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DBContextConnection") ?? throw new InvalidOperationException("Connection string 'DBContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IDocRepository, DocRepository>();
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDBContext>();


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireUppercase = false;
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
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
	var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

	var roles = new[] { "Admin", "Doctor", "Patient" };

	foreach (var role in roles)
	{
		if (!await roleManager.RoleExistsAsync(role))
		{
			await roleManager.CreateAsync(new IdentityRole(role));
		}
	}
}

using (var scope = app.Services.CreateScope())
{
	var userManager =
		scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
	string email = "mahiabdul20@admin.com";
	string password = "Mahi.2003";

	if (await userManager.FindByEmailAsync(email) == null)
	{
		var user = new ApplicationUser();
		user.UserName = email;
		user.Email = email;


		await userManager.CreateAsync(user, password);

		await userManager.AddToRoleAsync(user, "Admin");
	}

}
app.Run();
