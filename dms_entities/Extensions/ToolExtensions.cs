using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class ToolExtensions
    {
        public static void Map(this Tool dbTool, Tool tool)
        {
            dbTool.Name = tool.Name;
            dbTool.Description = tool.Description;
            dbTool.Rules = tool.Rules;
            dbTool.State = tool.State;
            dbTool.UsedInDepartments = tool.UsedInDepartments;
            dbTool.UsedInJobBreakDown = tool.UsedInJobBreakDown;
            dbTool.TimeCreated = tool.TimeCreated;
            dbTool.Deleted = tool.Deleted;
            dbTool.TimeDeleted = tool.TimeDeleted;
        }
    }
}
