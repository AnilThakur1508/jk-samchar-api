using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKSamachar.DAL.Enitity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JKSamachar.DAL.Data
{
    public class JKSamacharContext : IdentityDbContext<AppUser>
    {
        public JKSamacharContext(DbContextOptions<JKSamacharContext> options) : base(options) { }
        public DbSet<JKNews> JKNews { get; set; }
    }
}
