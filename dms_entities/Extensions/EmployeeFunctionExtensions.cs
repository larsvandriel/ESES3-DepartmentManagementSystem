using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class EmployeeFunctionExtensions
    {
        public static void Map(this EmployeeFunction dbEmployeeFunction, EmployeeFunction employeeFunction)
        {
            dbEmployeeFunction.Name = employeeFunction.Name;
            dbEmployeeFunction.Description = employeeFunction.Description;
            dbEmployeeFunction.Departments = employeeFunction.Departments;
            dbEmployeeFunction.Employees = employeeFunction.Employees;
            dbEmployeeFunction.RulesToFollow = employeeFunction.RulesToFollow;
            dbEmployeeFunction.TimeCreated = employeeFunction.TimeCreated;
            dbEmployeeFunction.Deleted = employeeFunction.Deleted;
            dbEmployeeFunction.TimeDeleted = employeeFunction.TimeDeleted;
        }
    }
}
