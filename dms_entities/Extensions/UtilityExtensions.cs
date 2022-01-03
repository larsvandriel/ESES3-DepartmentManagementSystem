using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class UtilityExtensions
    {
        public static void Map(this Utility dbUtility, Utility utility)
        {
            dbUtility.Name = utility.Name;
            dbUtility.Description = utility.Description;
            dbUtility.Rules = utility.Rules;
            dbUtility.State = utility.State;
            dbUtility.ProvidedByDepartments = utility.ProvidedByDepartments;
            dbUtility.TimeCreated = utility.TimeCreated;
            dbUtility.Deleted = utility.Deleted;
            dbUtility.TimeDeleted = utility.TimeDeleted;
        }
    }
}
