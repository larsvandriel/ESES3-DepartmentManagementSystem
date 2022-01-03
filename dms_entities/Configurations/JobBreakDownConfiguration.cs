using DepartmentManagementSystem.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Configurations
{
    public class JobBreakDownConfiguration : IEntityTypeConfiguration<JobBreakDown>
    {
        public void Configure(EntityTypeBuilder<JobBreakDown> builder)
        {
            builder.HasKey(jbd => jbd.Id);
            builder.HasMany(jbd => jbd.RequiredTools).WithMany(t => t.UsedInJobBreakDown);
            builder.HasMany(jbd => jbd.RequiredEquipment).WithMany(e => e.UsedInJobBreakDown);
            builder.HasMany(jbd => jbd.Steps).WithOne().IsRequired();
            builder.HasOne(jbd => jbd.Task).WithOne().IsRequired();
            builder.Property(jbd => jbd.TimeCreated).IsRequired();
            builder.Property(jbd => jbd.TimeEdited).IsRequired();
            builder.Property(jbd => jbd.Deleted).IsRequired();
            builder.Property(jbd => jbd.TimeDeleted);
        }
    }
}
