using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public int EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
        public List<EmployeeFunction> Functions { get; set; }
    }
}
