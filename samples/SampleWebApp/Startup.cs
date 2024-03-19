using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SampleWebApp.Authentication;
using SampleWebApp.Authentication.Jwt;
using SampleWebApp.Data;
using SampleWebApp.Services.HostedServices;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MsSqlServerProvider;
using Serilog.Ui.Web;
using Serilog.Ui.Web.Extensions;

namespace SampleWebApp
{
    public class Startup(IConfiguration configuration)
    {

        public IConfiguration Configuration { get; } = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            AddAuthentication(services);

            services.AddScoped<JwtTokenGenerator>();

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSerilogUi(options => options
                /* samples: authorization filters */
                // custom filter, sync
                .AddScopedSyncAuthFilter<SerilogUiCustomAuthFilter>()
                // default filter, async, checking a configured authorization policy
                .AddScopedPolicyAuthFilter("test-policy")

                /* sample: sql server - multiple registration */
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), "Logs")
                .UseSqlServer(Configuration.GetConnectionString("SecondLogConnection"), "Logs2")
            );

            services.AddSwaggerGen();

            // Generate dummy logs
            services.AddHostedService<SecondLogDummyLogGeneratorBackgroundService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSerilogUi(options =>
            {
                options.HomeUrl = "/#Test";
                options.InjectJavascript("/js/serilog-ui/custom.js");
                options.Authorization = new AuthorizationOptions
                {
                    AuthenticationType = AuthenticationType.Jwt,
                };
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private void AddAuthentication(IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy("test-policy", (builder) =>
                    {
                        // a sample policy, checking for a custom claim
                        builder.RequireClaim("example", "my-value");
                    });
                })
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidAudience = Configuration["Jwt:Audience"],
                            IssuerSigningKey = JwtKeyGenerator.Generate(Configuration["Jwt:SecretKey"])
                        };
                });
        }
    }
}