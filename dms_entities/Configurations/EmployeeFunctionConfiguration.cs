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
    public class EmployeeFunctionConfiguration : IEntityTypeConfiguration<EmployeeFunction>
    {
        public void Configure(EntityTypeBuilder<EmployeeFunction> builder)
        {
            builder.HasKey(ef => ef.Id);
            builder.HasMany(ef => ef.Departments).WithMany(d => d.EmployeeFunctions);
            builder.HasMany(ef => ef.Employees).WithMany(e => e.Functions);
            builder.HasMany(ef => ef.RulesToFollow).WithMany(r => r.ApplicableTo);
            builder.Property(ef => ef.Name).IsRequired();
            builder.Property(ef => ef.Description).IsRequired();
            builder.Property(ef => ef.TimeCreated).IsRequired();
            builder.Property(ef => ef.Deleted).IsRequired();
            builder.Property(ef => ef.TimeDeleted);
        }
    }
}
