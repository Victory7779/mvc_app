using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
//using mvc_app.Service;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //Add Service products Id
            builder.Services.AddScoped<IServiceProduct, ServiceProducts>();
            builder.Services.AddDbContext<ProductContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                }
                );
        //Identity Context
            builder.Services.AddDbContext<UserContext>(
                options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
            
            builder.Services.AddDefaultIdentity<IdentityUser>(
                options =>
                //confirmed email
                { options.SignIn.RequireConfirmedEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredUniqueChars = 0;
                }
                )
                .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<UserContext>();
        //MVC
        builder.Services.AddControllersWithViews();
            var app = builder.Build();
        app.UseRouting();//перекидывать будут на контролер Important include first
        app.UseAuthentication();//next
        app.UseAuthorization();//next
            app.UseStaticFiles();
            
            app.MapControllerRoute(
                name:"default",
                pattern:"{controller=Home}/{action=Index}/{id?}"
                );

            app.Run();
        }
    }

