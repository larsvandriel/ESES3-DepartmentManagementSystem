using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class JobBreakDownExtensions
    {
        public static void Map(this JobBreakDown dbJobBreakDown, JobBreakDown jobBreakDown)
        {
            dbJobBreakDown.Task = jobBreakDown.Task;
            dbJobBreakDown.RequiredTools = jobBreakDown.RequiredTools;
            dbJobBreakDown.RequiredEquipment = jobBreakDown.RequiredEquipment;
            dbJobBreakDown.Steps = jobBreakDown.Steps;
            dbJobBreakDown.TimeCreated = jobBreakDown.TimeCreated;
            dbJobBreakDown.TimeEdited = jobBreakDown.TimeEdited;
            dbJobBreakDown.Deleted = jobBreakDown.Deleted;
            dbJobBreakDown.TimeDeleted = jobBreakDown.TimeDeleted;
        }
    }
}
