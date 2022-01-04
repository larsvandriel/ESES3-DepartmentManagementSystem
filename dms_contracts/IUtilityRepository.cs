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
    public interface IUtilityRepository: IRepositoryBase<Utility>
    {
        PagedList<ShapedEntity> GetAllUtilities(UtilityParameters utilityParameters);
        ShapedEntity GetUtilityById(Guid utilityId, string fields);
        Utility GetUtilityById(Guid utilityId);
        void CreateUtility(Utility utility);
        void UpdateUtility(Utility dbUtility, Utility utility);
        void DeleteUtility(Utility utility);
    }
}
