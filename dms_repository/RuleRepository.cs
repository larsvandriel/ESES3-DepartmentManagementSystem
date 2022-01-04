using DepartmentManagementSystem.Contracts;
using DepartmentManagementSystem.Entities;
using DepartmentManagementSystem.Entities.Extensions;
using DepartmentManagementSystem.Entities.Helpers;
using DepartmentManagementSystem.Entities.Models;
using DepartmentManagementSystem.Entities.Parameters;
using DepartmentManagementSystem.Entities.ShapedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Repository
{
    public class RuleRepository: RepositoryBase<Rule>, IRuleRepository
    {
        private readonly ISortHelper<Rule> _sortHelper;

        private readonly IDataShaper<Rule> _dataShaper;

        public RuleRepository(RepositoryContext repositoryContext, ISortHelper<Rule> sortHelper, IDataShaper<Rule> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public void CreateRule(Rule rule)
        {
            rule.TimeCreated = DateTime.Now;
            Create(rule);
        }

        public void DeleteRule(Rule rule)
        {
            rule.Deleted = true;
            rule.TimeDeleted = DateTime.Now;
            Rule dbRule = GetRuleById(rule.Id);
            UpdateRule(dbRule, rule);
        }

        public PagedList<ShapedEntity> GetAllRules(RuleParameters ruleParameters)
        {
            var rules = FindByCondition(rule => !rule.Deleted);

            SearchByName(ref rules, ruleParameters.Name);

            var sortedRules = _sortHelper.ApplySort(rules, ruleParameters.OrderBy);
            var shapedRules = _dataShaper.ShapeData(sortedRules, ruleParameters.Fields).AsQueryable();

            return PagedList<ShapedEntity>.ToPagedList(shapedRules, ruleParameters.PageNumber, ruleParameters.PageSize);
        }

        public ShapedEntity GetRuleById(Guid ruleId, string fields)
        {
            var rule = FindByCondition(rule => rule.Id.Equals(ruleId)).FirstOrDefault();

            if (rule == null)
            {
                rule = new Rule();
            }

            return _dataShaper.ShapeData(rule, fields);
        }

        public Rule GetRuleById(Guid ruleId)
        {
            return FindByCondition(i => i.Id.Equals(ruleId)).FirstOrDefault();
        }

        public void UpdateRule(Rule dbRule, Rule rule)
        {
            dbRule.Map(rule);
            Update(dbRule);
        }

        private void SearchByName(ref IQueryable<Rule> rules, string ruleName)
        {
            if (!rules.Any() || string.IsNullOrWhiteSpace(ruleName))
            {
                return;
            }

            rules = rules.Where(i => i.Name.ToLower().Contains(ruleName.Trim().ToLower()));
        }
    }
}
