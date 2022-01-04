using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class StepExtensions
    {
        public static void Map(this Step dbStep, Step step)
        {
            dbStep.MainStep = step.MainStep;
            dbStep.Name = step.Name;
            dbStep.Description = step.Description;
        }
    }
}
