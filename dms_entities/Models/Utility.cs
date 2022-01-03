using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Models
{
    public class Utility
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Rule> Rules { get; set; }
        public StateOfBeing State { get; set; }
    }
}
