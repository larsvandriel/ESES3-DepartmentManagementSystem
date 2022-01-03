using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DepartmentManagementSystem.Entities.Models
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Employee HeadOfDepartment { get; set; }
        public List<Employee> TeamLeads { get; set; }
        public List<Employee> WorkerEmployees { get; set; }
        public List<EmployeeFunction> EmployeeFunctions { get; set; }
        public Location Location { get; set; }
        public List<Room> Rooms { get; set; }
        public FinanceController FinanceController { get; set; }
        public List<Task> Tasks { get; set; }
        public List<Equipment> Equipment { get; set; }
        public List<Facility> Facilities { get; set; }
        public List<Tool> Tools { get; set; }
        public List<Utility> Utilities { get; set; }
        public List<Archive> Archives { get; set; }
    }
}
