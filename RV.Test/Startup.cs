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
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;

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

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
            });

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Info
                    {
                        Title = "RV Test Web API",
                        Version = "1.0",
                        Description = "Web API with JWT auth, ASP.NET Core, SQL Server and Docker",
                        Contact = new Contact
                        {
                            Name = "Mateus Hortêncio dos Santos",
                            Url = "https://github.com/mateushortenciodossantos",
                            Email = "mateushortenciodossantos@gmail.com"
                        }
                    }
                );

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                string appBasePath =
                    PlatformServices.Default.Application.ApplicationBasePath;
                string appName =
                    PlatformServices.Default.Application.ApplicationName;
                string pathXml =
                    Path.Combine(appBasePath, $"{appName}.xml");

                c.IncludeXmlComments(pathXml);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<RvTestContext>();
                context.Database.EnsureCreated();
            }

            app.UseCors("CorsPolicy");

            app.UseMvc();

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "RV Test");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
