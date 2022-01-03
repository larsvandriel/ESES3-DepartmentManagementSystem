using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Models
{
    public class Rule
    {
        public Guid Id { get; set; }
        public string RuleCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<EmployeeFunction> ApplicableTo { get; set; }
        public List<Department> AppliedOn { get; set; }
        public DateTime TimeCreated { get; set; }
        public bool Deleted { get; set; }
        public DateTime TimeDeleted { get; set; }
    }
}
