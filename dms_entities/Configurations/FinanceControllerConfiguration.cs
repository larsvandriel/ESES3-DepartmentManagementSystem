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
    public class FinanceControllerConfiguration : IEntityTypeConfiguration<FinanceController>
    {
        public void Configure(EntityTypeBuilder<FinanceController> builder)
        {
            builder.HasKey(fc => fc.Id);
            builder.Property(fc => fc.Name).IsRequired();
        }
    }
}
