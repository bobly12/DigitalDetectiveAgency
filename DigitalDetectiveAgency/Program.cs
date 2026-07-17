using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DigitalDetectiveAgency.Models.Entities;
using DigitalDetectiveAgency.Data;
using DigitalDetectiveAgency.Repositories.Interfaces;
using DigitalDetectiveAgency.Repositories.Implementations;
using DigitalDetectiveAgency.Services.Interfaces;
using DigitalDetectiveAgency.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CORE WEB ROUTING (HYBRID API + MVC) ---
builder.Services.AddControllersWithViews(); 
builder.Services.AddEndpointsApiExplorer();

// --- 2. REGISTER SWAGGER DOCUMENTATION ---
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Digital Detective Agency API",
        Version = "v1",
        Description = "Backend engine routing for investigating cases and tracking player scores."
    });
});

// --- 3. DATABASE CONTEXT CONFIGURATION ---
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 4. REPOSITORY & ENGINE INJECTIONS ---
builder.Services.AddScoped<ICaseRepository, CaseRepository>();
builder.Services.AddScoped<IEvidenceRepository, EvidenceRepository>();
builder.Services.AddScoped<IWitnessRepository, WitnessRepository>();
builder.Services.AddScoped<ISuspectRepository, SuspectRepository>();
builder.Services.AddScoped<ICaseAttemptRepository, CaseAttemptRepository>();

// Service Layer Injections
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<IScoringService, ScoringService>();

// --- 5. AUTOMAPPER & IDENTITY LIFETIMES ---
builder.Services.AddAutoMapper(typeof(DigitalDetectiveAgency.Mappings.MappingProfile));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        else
        {
            context.Response.Redirect("/Account/Login");
        }
        return Task.CompletedTask;
    };
});

var app = builder.Build();

// --- 6. CONFIGURE THE HTTP PIPELINE & SWAGGER MIDDLEWARE ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Digital Detective Agency API v1");
        options.RoutePrefix = "swagger"; 
    });
}
else
{
    app.UseHsts();
}

// -------------------------------------------------------------
// FIX: ENABLE STATIC FILES SEEDING (Serves your CSS and JS layout files)
// -------------------------------------------------------------
app.UseStaticFiles();

// --- 7. AUTHENTICATION & SECURITY CONTEXT ---
app.UseAuthentication(); 
app.UseAuthorization();

// --- 8. ENDPOINT EXTRACTIONS ---
app.MapControllers(); 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}"); // Note: Changed default to Dashboard so it boots directly into your game UI

// --- 9. SEED DATABASE ON STARTUP ---
await DbSeeder.SeedDataAsync(app);

app.Run();