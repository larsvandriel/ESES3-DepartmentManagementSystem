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
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);
            builder.HasOne(d => d.HeadOfDepartment).WithOne().IsRequired();
            builder.HasMany(d => d.TeamLeads).WithOne().IsRequired();
            builder.HasMany(d => d.WorkerEmployees).WithMany(e => e.WorksInDepartments);
            builder.HasMany(d => d.EmployeeFunctions).WithMany(e => e.Departments);
            builder.HasOne(d => d.Location).WithOne().IsRequired();
            builder.HasMany(d => d.Rooms).WithOne().IsRequired();
            builder.HasOne(d => d.FinanceController).WithOne().IsRequired();
            builder.HasMany(d => d.Tasks).WithMany(t => t.Departments);
            builder.HasMany(d => d.Equipment).WithMany(e => e.UsedInDepartments);
            builder.HasMany(d => d.Facilities).WithMany(f => f.UsedByDepartments);
            builder.HasMany(d => d.Tools).WithMany(t => t.UsedInDepartments);
            builder.HasMany(d => d.Utilities).WithMany(u => u.ProvidedByDepartments);
            builder.HasMany(d => d.Archives).WithOne();
            builder.HasMany(d => d.Rules).WithMany(r => r.AppliedOn);
            builder.Property(d => d.Name).IsRequired();
            builder.Property(d => d.Email).IsRequired();
            builder.Property(d => d.PhoneNumber).IsRequired();
            builder.Property(d => d.TimeCreated).IsRequired();
            builder.Property(d => d.Deleted).IsRequired();
            builder.Property(d => d.TimeDeleted);
        }
    }
}
