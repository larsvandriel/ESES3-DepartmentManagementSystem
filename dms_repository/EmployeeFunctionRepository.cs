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
    public class EmployeeFunctionRepository: RepositoryBase<EmployeeFunction>, IEmployeeFunctionRepository
    {
        private readonly ISortHelper<EmployeeFunction> _sortHelper;

        private readonly IDataShaper<EmployeeFunction> _dataShaper;

        public EmployeeFunctionRepository(RepositoryContext repositoryContext, ISortHelper<EmployeeFunction> sortHelper, IDataShaper<EmployeeFunction> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public void CreateEmployeeFunction(EmployeeFunction employeeFunction)
        {
            employeeFunction.TimeCreated = DateTime.Now;
            Create(employeeFunction);
        }

        public void DeleteEmployeeFunction(EmployeeFunction employeeFunction)
        {
            employeeFunction.Deleted = true;
            employeeFunction.TimeDeleted = DateTime.Now;
            EmployeeFunction dbEmployeeFunction = GetEmployeeFunctionById(employeeFunction.Id);
            UpdateEmployeeFunction(dbEmployeeFunction, employeeFunction);
        }

        public PagedList<ShapedEntity> GetAllEmployeeFunctions(EmployeeFunctionParameters employeeFunctionParameters)
        {
            var employeeFunctions = FindByCondition(employeeFunction => !employeeFunction.Deleted);

            SearchByName(ref employeeFunctions, employeeFunctionParameters.Name);

            var sortedEmployeeFunctions = _sortHelper.ApplySort(employeeFunctions, employeeFunctionParameters.OrderBy);
            var shapedEmployeeFunctions = _dataShaper.ShapeData(sortedEmployeeFunctions, employeeFunctionParameters.Fields).AsQueryable();

            return PagedList<ShapedEntity>.ToPagedList(shapedEmployeeFunctions, employeeFunctionParameters.PageNumber, employeeFunctionParameters.PageSize);
        }

        public ShapedEntity GetEmployeeFunctionById(Guid employeeFunctionId, string fields)
        {
            var employeeFunction = FindByCondition(employeeFunction => employeeFunction.Id.Equals(employeeFunctionId)).FirstOrDefault();

            if (employeeFunction == null)
            {
                employeeFunction = new EmployeeFunction();
            }

            return _dataShaper.ShapeData(employeeFunction, fields);
        }

        public EmployeeFunction GetEmployeeFunctionById(Guid employeeFunctionId)
        {
            return FindByCondition(i => i.Id.Equals(employeeFunctionId)).FirstOrDefault();
        }

        public void UpdateEmployeeFunction(EmployeeFunction dbEmployeeFunction, EmployeeFunction employeeFunction)
        {
            dbEmployeeFunction.Map(employeeFunction);
            Update(dbEmployeeFunction);
        }

        private void SearchByName(ref IQueryable<EmployeeFunction> employeeFunctions, string employeeFunctionName)
        {
            if (!employeeFunctions.Any() || string.IsNullOrWhiteSpace(employeeFunctionName))
            {
                return;
            }

            employeeFunctions = employeeFunctions.Where(i => i.Name.ToLower().Contains(employeeFunctionName.Trim().ToLower()));
        }
    }
}
