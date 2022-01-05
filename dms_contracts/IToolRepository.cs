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
    public interface IToolRepository: IRepositoryBase<Tool>
    {
        PagedList<ShapedEntity> GetAllTools(ToolParameters toolParameters);
        ShapedEntity GetToolById(Guid toolId, string fields);
        Tool GetToolById(Guid toolId);
        void CreateTool(Tool tool);
        void UpdateTool(Tool dbTool, Tool tool);
        void DeleteTool(Tool tool);
    }
}
