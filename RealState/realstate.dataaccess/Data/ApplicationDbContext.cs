using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using realstate.models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace realstate.dataaccess.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {

        }
        public DbSet<User> UserTBL { get; set; } 
        public DbSet<Location> LocationTBL { get; set; }
        public DbSet<Society> SocietyTBL { get; set; }
        public DbSet<ApplicationUser> ApplicationUserTBL { get; set; }
        public DbSet<VerifiedUser> VerifiedUserTBL { get; set; }
        public DbSet<Plot> PlotTBL { get; set; }
    }
}
