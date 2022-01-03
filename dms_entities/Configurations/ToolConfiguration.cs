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
    public class ToolConfiguration : IEntityTypeConfiguration<Tool>
    {
        public void Configure(EntityTypeBuilder<Tool> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasMany(t => t.Rules).WithOne();
            builder.HasMany(t => t.UsedInDepartments).WithMany(d => d.Tools);
            builder.HasMany(t => t.UsedInJobBreakDown).WithMany(jbd => jbd.RequiredTools);
            builder.Property(t => t.Name).IsRequired();
            builder.Property(t => t.Description).IsRequired();
            builder.Property(t => t.State).IsRequired();
            builder.Property(t => t.TimeCreated).IsRequired();
            builder.Property(t => t.Deleted).IsRequired();
            builder.Property(t => t.TimeDeleted).IsRequired();
        }
    }
}
