using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class FinanceControllerExtensions
    {
        public static void Map(this FinanceController dbFinanceController, FinanceController financeController)
        {
            dbFinanceController.Name = financeController.Name;
        }
    }
}
