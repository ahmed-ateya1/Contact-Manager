using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.AppDbContext;
using Repository;
using RepositoryContract;
using ServiceContract;
using Services;
using Microsoft.AspNetCore.Authorization;
using ContactsManager.Core.Domain.IdentityEntites;

namespace ContactAppManager
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure the DbContext with SQL Server
            services.AddDbContext<PersonDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("connstr"));
            });

            // Add MVC services to the container
            services.AddControllersWithViews();

            // Register application services
            services.AddScoped<IPersonsServices, PersonsService>();
            services.AddScoped<ICountriesServices, CountriesServices>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();

            // Configure Identity services
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 3;
            })
            .AddEntityFrameworkStores<PersonDbContext>()
            .AddDefaultTokenProviders()
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, PersonDbContext, Guid>>()
            .AddRoleStore<RoleStore<ApplicationRole, PersonDbContext, Guid>>();

            // Configure Authorization
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("NotAuthentication", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return !context.User.Identity.IsAuthenticated;
                    });
                });
                options.AddPolicy("NotAdmin", policy =>
                {
                    policy.RequireAssertion(context =>
                    {
                        return !context.User.IsInRole("Admin");
                    });
                });
            });

            // Configure Application Cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });


            return services;
        }
    }
}
