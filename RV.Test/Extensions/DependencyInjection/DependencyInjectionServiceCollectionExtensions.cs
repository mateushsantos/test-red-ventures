using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RV.Test.Infra.Repositories;
using RV.Test.Web.Context;
using RV.Test.Web.Services;

namespace RV.Test.Web.Extensions.DependencyInjection
{
    public static class DependencyInjectionServiceCollectionExtensions
    {
        public static void RegisterDependencyInjection(this IServiceCollection service)
        {
            service.AddTransient(typeof(IRepository<>), typeof(SqlServerRepository<>));
            service.AddTransient<DbContext, RvTestContext>();
            service.AddTransient<JwtAuthenticationService>();
        }
    }
}
