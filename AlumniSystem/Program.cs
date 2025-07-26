using AlumniSystem.Core.Interfaces;
using AlumniSystem.Core.Services;
using AlumniSystem.Infrastructure.Data;
using AlumniSystem.Infrastructure.Interfaces;
using AlumniSystem.Infrastructure.Models;
using AlumniSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AlumniDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
	options.Password.RequireDigit = true;
	options.Password.RequiredLength = 6;
	options.Password.RequireNonAlphanumeric = true;
	options.Password.RequireUppercase = true;
	options.Password.RequireLowercase = true;

})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AlumniDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAlumniRepository, AlumniRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IJobPostingRepository, JobPostingRepository>();
builder.Services.AddScoped<ICommunityRepository, CommunityRepository>();

builder.Services.AddScoped<IAlumniService, AlumniService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IJobPostingService, JobPostingService>();
builder.Services.AddScoped<ICommunityService, CommunityService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
	foreach (var role in new[] { "Admin", "User" })
	{
		if (!await roleMgr.RoleExistsAsync(role))
		{
			await roleMgr.CreateAsync(new IdentityRole(role));
		}
	}

	var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
	const string adminEmail = "antoan2_f@abv.bg";
	const string adminPassword = "Admin123!";

	var adminUser = await userMgr.FindByEmailAsync(adminEmail);
	if (adminUser == null)
	{
		adminUser = new ApplicationUser
		{
			UserName = adminEmail,
			Email = adminEmail,
			EmailConfirmed = true
		};
		var result = await userMgr.CreateAsync(adminUser, adminPassword);
		if (result.Succeeded)
		{
			await userMgr.AddToRoleAsync(adminUser, "Admin");
		}
		else
		{
			throw new Exception($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
		}
	}

	// Configure the HTTP request pipeline.
	if (app.Environment.IsDevelopment())
	{
		app.UseMigrationsEndPoint();
	}
	else
	{
		app.UseExceptionHandler("/Home/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	}

	app.UseHttpsRedirection();
	app.UseStaticFiles();

	app.UseRouting();

	app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");
	app.MapRazorPages();

	app.Run();
}