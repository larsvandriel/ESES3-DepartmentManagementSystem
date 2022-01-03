using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Rule> Rules { get; set; }
        public StateOfBeing State { get; set; }
        public List<Department> UsedInDepartments { get; set; }
        public List<JobBreakDown> UsedInJobBreakDown { get; set; }
        public DateTime TimeCreated { get; set; }
        public bool Deleted { get; set; }
        public DateTime TimeDeleted { get; set; }
    }
}
