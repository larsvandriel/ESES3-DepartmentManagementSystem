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
    public class UtilityRepository: RepositoryBase<Utility>, IUtilityRepository
    {
        private readonly ISortHelper<Utility> _sortHelper;

        private readonly IDataShaper<Utility> _dataShaper;

        public UtilityRepository(RepositoryContext repositoryContext, ISortHelper<Utility> sortHelper, IDataShaper<Utility> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public void CreateUtility(Utility utility)
        {
            utility.TimeCreated = DateTime.Now;
            Create(utility);
        }

        public void DeleteUtility(Utility utility)
        {
            utility.Deleted = true;
            utility.TimeDeleted = DateTime.Now;
            Utility dbUtility = GetUtilityById(utility.Id);
            UpdateUtility(dbUtility, utility);
        }

        public PagedList<ShapedEntity> GetAllUtilities(UtilityParameters utilityParameters)
        {
            var utilities = FindByCondition(utility => !utility.Deleted);

            SearchByName(ref utilities, utilityParameters.Name);

            var sortedUtilities = _sortHelper.ApplySort(utilities, utilityParameters.OrderBy);
            var shapedUtilities = _dataShaper.ShapeData(sortedUtilities, utilityParameters.Fields).AsQueryable();

            return PagedList<ShapedEntity>.ToPagedList(shapedUtilities, utilityParameters.PageNumber, utilityParameters.PageSize);
        }

        public ShapedEntity GetUtilityById(Guid utilityId, string fields)
        {
            var utility = FindByCondition(utility => utility.Id.Equals(utilityId)).FirstOrDefault();

            if (utility == null)
            {
                utility = new Utility();
            }

            return _dataShaper.ShapeData(utility, fields);
        }

        public Utility GetUtilityById(Guid utilityId)
        {
            return FindByCondition(i => i.Id.Equals(utilityId)).FirstOrDefault();
        }

        public void UpdateUtility(Utility dbUtility, Utility utility)
        {
            dbUtility.Map(utility);
            Update(dbUtility);
        }

        private void SearchByName(ref IQueryable<Utility> utilities, string utilityName)
        {
            if (!utilities.Any() || string.IsNullOrWhiteSpace(utilityName))
            {
                return;
            }

            utilities = utilities.Where(i => i.Name.ToLower().Contains(utilityName.Trim().ToLower()));
        }
    }
}
