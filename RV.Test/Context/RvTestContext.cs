using Microsoft.EntityFrameworkCore;
using RV.Test.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RV.Test.Web.Context
{
    public class RvTestContext : DbContext
    {
        public RvTestContext(DbContextOptions<RvTestContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admin { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Widget> Widget { get; set; }
    }
}
