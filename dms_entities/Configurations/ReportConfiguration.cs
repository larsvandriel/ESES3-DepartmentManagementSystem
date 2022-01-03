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
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.HasKey(r => r.Id);
            builder.HasOne(r => r.WrittenBy).WithMany().IsRequired();
            builder.Property(r => r.Name).IsRequired();
            builder.Property(r => r.Title).IsRequired();
            builder.Property(r => r.Content).IsRequired();
            builder.Property(r => r.TimeWritten).IsRequired();
            builder.Property(r => r.FilePath).IsRequired();
            builder.Property(r => r.TimeEdited).IsRequired();
            builder.Property(r => r.Deleted).IsRequired();
            builder.Property(r => r.TimeDeleted);
        }
    }
}
