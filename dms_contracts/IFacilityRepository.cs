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
    public interface IFacilityRepository: IRepositoryBase<Facility>
    {
        PagedList<ShapedEntity> GetAllFacilities(FacilityParameters facilityParameters);
        ShapedEntity GetFacilityById(Guid facilityId, string fields);
        Facility GetFacilityById(Guid facilityId);
        void CreateFacility(Facility facility);
        void UpdateFacility(Facility dbFacility, Facility facility);
        void DeleteFacility(Facility facility);
    }
}
