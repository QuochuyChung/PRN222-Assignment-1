using asg1.BLL;
using asg1.BLL.AI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddBusinessLayer(builder.Configuration);

builder.Services.Configure<GeminiOptions>(builder.Configuration.GetSection(GeminiOptions.SectionName));
builder.Services.AddHttpClient<IAiQuestionGenerator, GeminiQuestionGenerator>(client =>
{
    client.Timeout = TimeSpan.FromMinutes(2);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

try
{
    await app.Services.SeedDataAsync();
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "Subject seed skipped - database unavailable.");
}

app.Run();
