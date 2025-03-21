using RestAPIAccountManagement.DAL;
using RestAPIAccountManagement.Hashing;

namespace RestAPIAccountManagement;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddScoped<CreateDatabase>();
        
        var pepper = builder.Configuration.GetValue<string>("Secret:Pepper")?? "Secure Value";
        HashHelper.Pepper = pepper;

        var conString = builder.Configuration.GetConnectionString("DefaultConnection");
        AccountDAL.ConnectionString = conString;
        
        var app = builder.Build();
       
        using (var scope = app.Services.CreateScope())
        {
            try
            {
                var createDatabase = scope.ServiceProvider.GetRequiredService<CreateDatabase>();
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

                bool success = await createDatabase.StartCreateDatabaseAsync(connectionString);
                if (!success)
                {
                    Console.WriteLine("Error setting up the database.");
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error setting up the database: {e.Message}");
                throw;
            }
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}