using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RV.Test.Infra.Repositories;
using RV.Test.Web.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RV.Test.Web.Extensions.DependencyInjection
{
    public static class DependencyInjectionServiceCollectionExtensions
    {
        public static void RegisterDependencyInjection(this IServiceCollection service)
        {
            service.AddTransient(typeof(IRepository<>), typeof(SqlServerRepository<>));
            service.AddTransient<DbContext, RvTestContext>();
        }
    }
}
