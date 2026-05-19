using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Infrastructure.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Tables 
        public  DbSet<Project> Projects { get; set; }
        public  DbSet<TaskItem> TaskItems { get; set; }

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ProjectConfiguration());
            builder.ApplyConfiguration(new TaskItemConfiguration());

        }

    }
}
