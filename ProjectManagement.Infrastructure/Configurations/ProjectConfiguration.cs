using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Infrastructure.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
       

        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(p=>p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p=>p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.CreatedAt)
                 .IsRequired();

           // one  User => many Projects
            builder.HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
