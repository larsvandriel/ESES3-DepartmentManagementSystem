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
    public class UtilityConfiguration : IEntityTypeConfiguration<Utility>
    {
        public void Configure(EntityTypeBuilder<Utility> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasMany(u => u.Rules).WithOne();
            builder.HasMany(u => u.ProvidedByDepartments).WithMany(d => d.Utilities);
            builder.Property(u => u.Name).IsRequired();
            builder.Property(u => u.Description).IsRequired();
            builder.Property(u => u.State).IsRequired();
            builder.Property(u => u.TimeCreated).IsRequired();
            builder.Property(u => u.Deleted).IsRequired();
            builder.Property(u => u.TimeDeleted);
        }
    }
}
