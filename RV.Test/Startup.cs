using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RV.Test.Web.Extensions.DependencyInjection;
using RV.Test.Web.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RV.Test.Web.Authentication;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace RV.Test
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private const string SecretKey = "redventuressupersecretkeynooneshouldknow123!!!@@@";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.RegisterDependencyInjection();

            var connection = Configuration.GetConnectionString("SqlConnection");

            services.AddDbContext<RvTestContext>(
                options => options.UseSqlServer(connection));

            services.AddAuthorization(options =>
            {
                options.AddPolicy("SystemAdmin",
                    policy => policy.RequireClaim("LoggedSystemAdmin", "Admin"));
            });

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<RvTestContext>();
                context.Database.EnsureCreated();
            }

            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
            //}

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            app.AddJwtBearer(new JwtBearerOptions
            {
                TokenValidationParameters = tokenValidationParameters
            });

            app.UseMvc(routes => {
                routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action}/{id?}");
            });
        }
    }
}
