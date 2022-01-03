using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = DepartmentManagementSystem.Entities.Models.Task;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class TaskExtensions
    {
        public static void Map(this Task dbTask, Task task)
        {
            dbTask.Name = task.Name;
            dbTask.Description = task.Description;
            dbTask.JobBreakDown = task.JobBreakDown;
            dbTask.RequiredFunction = task.RequiredFunction;
            dbTask.Departments = task.Departments;
            dbTask.TimeCreated = task.TimeCreated;
            dbTask.Deleted = task.Deleted;
            dbTask.TimeDeleted = task.TimeDeleted;
        }
    }
}
