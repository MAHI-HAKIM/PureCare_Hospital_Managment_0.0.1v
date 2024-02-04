using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PureCareHub_HospitalCare.Data;
using PureCareHub_HospitalCare.Areas.Identity.Data;
using PureCareHub_HospitalCare.Models.Service;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DBContextConnection") ?? throw new InvalidOperationException("Connection string 'DBContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(connectionString));

//for  Multilingual Support and Localisation 

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();	

builder.Services.AddScoped<IDocRepository, DocRepository>();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDBContext>();


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = new[] { "en", "tr" };
	options.SetDefaultCulture(supportedCultures[0])
		.AddSupportedCultures(supportedCultures)
		.AddSupportedUICultures(supportedCultures);
});

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

app.UseRequestLocalization(new RequestLocalizationOptions
{
	ApplyCurrentCultureToResponseHeaders = true
});
app.UseRequestLocalization(options =>
{
	var questStringCultureProvider = options.RequestCultureProviders[0];
	options.RequestCultureProviders.RemoveAt(0);
	options.RequestCultureProviders.Insert(1, questStringCultureProvider);
});
app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
	{
		endpoints.MapControllerRoute(
			name: "api",
			pattern: "api/{controller=Service}/{action=Index}/{id?}");
	});

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

	string email = "b201210605@sakarya.edu.tr";
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
