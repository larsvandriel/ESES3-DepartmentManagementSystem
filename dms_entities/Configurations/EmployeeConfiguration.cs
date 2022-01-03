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
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Address).WithMany().IsRequired();
            builder.HasMany(e => e.WorksInDepartments).WithMany(d => d.WorkerEmployees);
            builder.HasMany(e => e.Functions).WithMany(ef => ef.Employees);
            builder.HasIndex(e => e.EmployeeNumber);
            builder.Property(e => e.Name);
            builder.Property(e => e.PhoneNumber);
        }
    }
}
