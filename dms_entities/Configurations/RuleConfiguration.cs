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
    public class RuleConfiguration : IEntityTypeConfiguration<Rule>
    {
        public void Configure(EntityTypeBuilder<Rule> builder)
        {
            builder.HasKey(r => r.Id);
            builder.HasMany(r => r.ApplicableTo).WithMany(ef => ef.RulesToFollow);
            builder.HasMany(r => r.AppliedOn).WithMany(d => d.Rules);
            builder.HasIndex(r => r.RuleCode).IsUnique();
            builder.Property(r => r.Name).IsRequired();
            builder.Property(r => r.Description).IsRequired();
            builder.Property(r => r.TimeCreated).IsRequired();
            builder.Property(r => r.Deleted).IsRequired();
            builder.Property(r => r.TimeDeleted);
        }
    }
}
