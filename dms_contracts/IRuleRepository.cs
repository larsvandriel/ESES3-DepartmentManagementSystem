using DepartmentManagementSystem.Entities.Helpers;
using DepartmentManagementSystem.Entities.Models;
using DepartmentManagementSystem.Entities.Parameters;
using DepartmentManagementSystem.Entities.ShapedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Contracts
{
    public interface IRuleRepository: IRepositoryBase<Rule>
    {
        PagedList<ShapedEntity> GetAllRules(RuleParameters ruleParameters);
        ShapedEntity GetRuleById(Guid ruleId, string fields);
        Rule GetRuleById(Guid ruleId);
        void CreateRule(Rule rule);
        void UpdateRule(Rule dbRule, Rule rule);
        void DeleteRule(Rule rule);
    }
}
