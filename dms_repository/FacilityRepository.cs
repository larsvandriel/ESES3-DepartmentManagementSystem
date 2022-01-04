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
    public class FacilityRepository: RepositoryBase<Facility>, IFacilityRepository
    {
        private readonly ISortHelper<Facility> _sortHelper;

        private readonly IDataShaper<Facility> _dataShaper;

        public FacilityRepository(RepositoryContext repositoryContext, ISortHelper<Facility> sortHelper, IDataShaper<Facility> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public void CreateFacility(Facility facility)
        {
            facility.TimeCreated = DateTime.Now;
            Create(facility);
        }

        public void DeleteFacility(Facility facility)
        {
            facility.Deleted = true;
            facility.TimeDeleted = DateTime.Now;
            Facility dbFacility = GetFacilityById(facility.Id);
            UpdateFacility(dbFacility, facility);
        }

        public PagedList<ShapedEntity> GetAllFacilities(FacilityParameters facilityParameters)
        {
            var facilities = FindByCondition(facility => !facility.Deleted);

            SearchByName(ref facilities, facilityParameters.Name);

            var sortedFacilities = _sortHelper.ApplySort(facilities, facilityParameters.OrderBy);
            var shapedFacilities = _dataShaper.ShapeData(sortedFacilities, facilityParameters.Fields).AsQueryable();

            return PagedList<ShapedEntity>.ToPagedList(shapedFacilities, facilityParameters.PageNumber, facilityParameters.PageSize);
        }

        public ShapedEntity GetFacilityById(Guid facilityId, string fields)
        {
            var facility = FindByCondition(facility => facility.Id.Equals(facilityId)).FirstOrDefault();

            if (facility == null)
            {
                facility = new Facility();
            }

            return _dataShaper.ShapeData(facility, fields);
        }

        public Facility GetFacilityById(Guid facilityId)
        {
            return FindByCondition(i => i.Id.Equals(facilityId)).FirstOrDefault();
        }

        public void UpdateFacility(Facility dbFacility, Facility facility)
        {
            dbFacility.Map(facility);
            Update(dbFacility);
        }

        private void SearchByName(ref IQueryable<Facility> facilities, string facilityName)
        {
            if (!facilities.Any() || string.IsNullOrWhiteSpace(facilityName))
            {
                return;
            }

            facilities = facilities.Where(i => i.Name.ToLower().Contains(facilityName.Trim().ToLower()));
        }
    }
}
