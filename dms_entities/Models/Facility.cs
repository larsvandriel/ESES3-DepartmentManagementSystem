using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Models
{
    public class Facility
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Department> UsedByDepartments { get; set; }
        public DateTime TimeCreated { get; set; }
        public bool Deleted { get; set; }
        public DateTime TimeDeleted { get; set; }
    }
}
