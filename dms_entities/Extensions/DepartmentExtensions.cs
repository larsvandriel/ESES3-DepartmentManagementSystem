using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class DepartmentExtensions
    {
        public static void Map(this Department dbDepartment, Department department)
        {
            dbDepartment.Name = department.Name;
            dbDepartment.Email = department.Email;
            dbDepartment.PhoneNumber = department.PhoneNumber;
            dbDepartment.HeadOfDepartment = department.HeadOfDepartment;
            dbDepartment.TeamLeads = department.TeamLeads;
            dbDepartment.WorkerEmployees = department.WorkerEmployees;
            dbDepartment.Location = department.Location;
            dbDepartment.Rooms = department.Rooms;
            dbDepartment.FinanceController = department.FinanceController;
            dbDepartment.Tasks = department.Tasks;
            dbDepartment.Equipment = department.Equipment;
            dbDepartment.Facilities = department.Facilities;
            dbDepartment.Tools = department.Tools;
            dbDepartment.Utilities = department.Utilities;
            dbDepartment.Archives = department.Archives;
            dbDepartment.Rules = department.Rules;
            dbDepartment.TimeCreated = department.TimeCreated;
            dbDepartment.Deleted = department.Deleted;
            dbDepartment.TimeDeleted = department.TimeDeleted;
        }
    }
}
