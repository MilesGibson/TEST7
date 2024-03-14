using DevExpress.Blazor.Reporting;
using DevExpress.XtraCharts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using TEST7.Areas.Identity;
using TEST7.Data;
using Blazored.Modal;
using Blazored.Toast;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
builder.Services.AddSingleton<WeatherForecastService>();

//
// Blazored
//
//builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredModal();
builder.Services.AddBlazoredToast();


//
// DevExpress
//
//
//
//https://supportcenter.devexpress.com/ticket/details/t1081253/the-location-of-css-resources-for-devexpress-blazor-controls-has-changed

DevExpress.Blazor.CompatibilitySettings.AddSpaceAroundFormLayoutContent = true;
builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5);
// March 5, 2023 -- review this, IMPORTANT!
// https://supportcenter.devexpress.com/ticket/details/t1019255/reporting-for-asp-net-core-blazor-server-the-report-designer-web-document-viewer-and
builder.Services.AddDevExpressBlazorReporting();
builder.Services.AddDevExpressServerSideBlazorReportViewer();

// OBSOLETE as of Dec 16, 2023 or before
//services.AddDevExpressBlazorWasmMasks();

builder.Services.Configure<DevExpress.Blazor.Configuration.GlobalOptions>(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
    options.ShowValidationIcon = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
