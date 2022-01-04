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
    public class ArchiveConfiguration : IEntityTypeConfiguration<Archive>
    {
        public void Configure(EntityTypeBuilder<Archive> builder)
        {
            builder.HasKey(a => a.Id);
            builder.HasMany(a => a.Reports).WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Property(a => a.Name).IsRequired();
            builder.Property(a => a.TimeCreated).IsRequired();
            builder.Property(a => a.Deleted).IsRequired();
            builder.Property(a => a.TimeDeleted);
        }
    }
}
