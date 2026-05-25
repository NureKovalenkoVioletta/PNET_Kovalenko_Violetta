using System.Globalization;
using DietaryFitnessProject.BLL.Interfaces;
using DietaryFitnessProject.BLL.Services;
using DietaryFitnessProject.DAL.Data;
using DietaryFitnessProject.DAL.Interfaces;
using DietaryFitnessProject.DAL.Repositories;
using DietaryFitnessProject.UI.Localization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

namespace DietaryFitnessProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var ukCulture = new CultureInfo("uk-UA");
            CultureInfo.DefaultThreadCurrentCulture = ukCulture;
            CultureInfo.DefaultThreadCurrentUICulture = ukCulture;

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(ukCulture);
                options.SupportedCultures = [ukCulture];
                options.SupportedUICultures = [ukCulture];
            });

            builder.Services
                .AddRazorPages(options => options.RootDirectory = "/UI/Pages")
                .AddMvcOptions(UkMvcValidation.ConfigureModelBindingMessages);
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth/Login";
                    options.LogoutPath = "/Auth/Logout";
                    options.AccessDeniedPath = "/Auth/Login";
                    options.SlidingExpiration = true;
                });
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

                
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IDailyDietPlanRepository, DailyDietPlanRepository>();
            builder.Services.AddScoped<IMealRepository, MealRepository>();
            builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IMealRecipeRepository, MealRecipeRepository>();
            builder.Services.AddScoped<IRecipeProductRepository, RecipeProductRepository>();


            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserProfileService, UserProfileService>();
            builder.Services.AddScoped<IDailyDietPlanService, DailyDietPlanService>();
            builder.Services.AddScoped<IMealService, MealService>();
            builder.Services.AddScoped<IRecipeService, RecipeService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IMealRecipeService, MealRecipeService>();
            builder.Services.AddScoped<IRecipeProductService, RecipeProductService>();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRequestLocalization();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
