using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class RuleExtensions
    {
        public static void Map(this Rule dbRule, Rule rule)
        {
            dbRule.RuleCode = rule.RuleCode;
            dbRule.Name = rule.Name;
            dbRule.Description = rule.Description;
            dbRule.ApplicableTo = rule.ApplicableTo;
            dbRule.AppliedOn = rule.AppliedOn;
            dbRule.TimeCreated = rule.TimeCreated;
            dbRule.Deleted = rule.Deleted;
            dbRule.TimeDeleted = rule.TimeDeleted;
        }
    }
}
