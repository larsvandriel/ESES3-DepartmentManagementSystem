using DepartmentManagementSystem.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task = DepartmentManagementSystem.Entities.Models.Task;

namespace DepartmentManagementSystem.Entities.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.JobBreakDown).WithOne().IsRequired();
            builder.HasOne(t => t.RequiredFunction).WithMany().IsRequired();
            builder.HasMany(t => t.Departments).WithMany(d => d.Tasks);
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.Description).IsRequired();
            builder.Property(t => t.TimeCreated).IsRequired();
            builder.Property(t => t.Deleted).IsRequired();
            builder.Property(t => t.TimeDeleted);
        }
    }
}
