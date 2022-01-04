using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class FacilityExtensions
    {
        public static void Map(this Facility dbFacility, Facility facility)
        {
            dbFacility.Name = facility.Name;
            dbFacility.Description = facility.Description;
            dbFacility.UsedByDepartments = facility.UsedByDepartments;
            dbFacility.TimeCreated = facility.TimeCreated;
            dbFacility.Deleted = facility.Deleted;
            dbFacility.TimeDeleted = facility.TimeDeleted;
        }
    }
}
