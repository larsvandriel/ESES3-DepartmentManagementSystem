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
    public class ToolRepository: RepositoryBase<Tool>, IToolRepository
    {
        private readonly ISortHelper<Tool> _sortHelper;

        private readonly IDataShaper<Tool> _dataShaper;

        public ToolRepository(RepositoryContext repositoryContext, ISortHelper<Tool> sortHelper, IDataShaper<Tool> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public void CreateTool(Tool tool)
        {
            tool.TimeCreated = DateTime.Now;
            Create(tool);
        }

        public void DeleteTool(Tool tool)
        {
            tool.Deleted = true;
            tool.TimeDeleted = DateTime.Now;
            Tool dbTool = GetToolById(tool.Id);
            UpdateTool(dbTool, tool);
        }

        public PagedList<ShapedEntity> GetAllTools(ToolParameters toolParameters)
        {
            var tools = FindByCondition(tool => !tool.Deleted);

            SearchByName(ref tools, toolParameters.Name);

            var sortedTools = _sortHelper.ApplySort(tools, toolParameters.OrderBy);
            var shapedTools = _dataShaper.ShapeData(sortedTools, toolParameters.Fields).AsQueryable();

            return PagedList<ShapedEntity>.ToPagedList(shapedTools, toolParameters.PageNumber, toolParameters.PageSize);
        }

        public ShapedEntity GetToolById(Guid toolId, string fields)
        {
            var tool = FindByCondition(tool => tool.Id.Equals(toolId)).FirstOrDefault();

            if (tool == null)
            {
                tool = new Tool();
            }

            return _dataShaper.ShapeData(tool, fields);
        }

        public Tool GetToolById(Guid toolId)
        {
            return FindByCondition(i => i.Id.Equals(toolId)).FirstOrDefault();
        }

        public void UpdateTool(Tool dbTool, Tool tool)
        {
            dbTool.Map(tool);
            Update(dbTool);
        }

        private void SearchByName(ref IQueryable<Tool> tools, string toolName)
        {
            if (!tools.Any() || string.IsNullOrWhiteSpace(toolName))
            {
                return;
            }

            tools = tools.Where(i => i.Name.ToLower().Contains(toolName.Trim().ToLower()));
        }
    }
}
