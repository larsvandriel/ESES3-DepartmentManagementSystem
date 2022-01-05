using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class EquipmentExtensions
    {
        public static void Map(Equipment dbEquipment, Equipment equipment)
        {
            dbEquipment.Name = equipment.Name;
            dbEquipment.Description = equipment.Description;
            dbEquipment.Rules = equipment.Rules;
            dbEquipment.State = equipment.State;
            dbEquipment.UsedInDepartments = equipment.UsedInDepartments;
            dbEquipment.UsedInJobBreakDown = equipment.UsedInJobBreakDown;
            dbEquipment.TimeCreated = equipment.TimeCreated;
            dbEquipment.Deleted = equipment.Deleted;
            dbEquipment.TimeDeleted = equipment.TimeDeleted;
        }
    }
}
