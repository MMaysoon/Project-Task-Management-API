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
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.ToTable("TaskItems");

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(150);


            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(t => t.DueDate)
                .IsRequired();


            // enum(status)
            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<int>(); 


            // enum(priority)
            builder.Property(t => t.Priority)
                .IsRequired()
                .HasConversion<int>();


            // many tasks => one Project 
            builder.HasOne(t => t.Project)
                .WithMany(p => p.TaskItems)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
