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
    public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
    {
        public void Configure(EntityTypeBuilder<Equipment> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasMany(e => e.Rules).WithOne();
            builder.HasMany(e => e.UsedInDepartments).WithMany(d => d.Equipment);
            builder.HasMany(e => e.UsedInJobBreakDown).WithMany(jbd => jbd.RequiredEquipment);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.State).IsRequired();
            builder.Property(e => e.TimeCreated).IsRequired();
            builder.Property(e => e.Deleted).IsRequired();
            builder.Property(e => e.TimeDeleted);
        }
    }
}
