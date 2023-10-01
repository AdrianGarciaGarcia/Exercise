using Serilog;
using UI.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    builder.Services.AddRazorPages();
    builder.Services.AddConfiguredApiService(builder.Configuration);
}
catch (Exception ex)
{
    Log.Fatal(ex, "An error occurred while configuring the application.");
    throw;
}

var app = builder.Build();

//Show page errors, this will only for production
//if (!app.Environment.IsDevelopment())
//{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
