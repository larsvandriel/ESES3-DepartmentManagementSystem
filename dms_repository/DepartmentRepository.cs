using DepartmentManagementSystem.Contracts;
using DepartmentManagementSystem.Entities;
using DepartmentManagementSystem.Entities.Extensions;
using DepartmentManagementSystem.Entities.Helpers;
using DepartmentManagementSystem.Entities.Models;
using DepartmentManagementSystem.Entities.Parameters;
using DepartmentManagementSystem.Entities.ShapedEntities;

namespace DepartmentManagementSystem.Repository
{
    public class DepartmentRepository: RepositoryBase<Department>, IDepartmentRepository
    {
        private readonly ISortHelper<Department> _sortHelper;

        private readonly IDataShaper<Department> _dataShaper;

        public DepartmentRepository(RepositoryContext repositoryContext, ISortHelper<Department> sortHelper, IDataShaper<Department> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public void CreateDepartment(Department department)
        {
            department.TimeCreated = DateTime.Now;
            Create(department);
        }

        public void DeleteDepartment(Department department)
        {
            department.Deleted = true;
            department.TimeDeleted = DateTime.Now;
            Department dbDepartment = GetDepartmentById(department.Id);
            UpdateDepartment(dbDepartment, department);
        }

        public PagedList<ShapedEntity> GetAllDepartments(DepartmentParameters departmentParameters)
        {
            var departments = FindByCondition(department => !department.Deleted);

            SearchByName(ref departments, departmentParameters.Name);

            var sortedDepartments = _sortHelper.ApplySort(departments, departmentParameters.OrderBy);
            var shapedDepartments = _dataShaper.ShapeData(sortedDepartments, departmentParameters.Fields).AsQueryable();

            return PagedList<ShapedEntity>.ToPagedList(shapedDepartments, departmentParameters.PageNumber, departmentParameters.PageSize);
        }

        public ShapedEntity GetDepartmentById(Guid departmentId, string fields)
        {
            var department = FindByCondition(department => department.Id.Equals(departmentId)).FirstOrDefault();

            if (department == null)
            {
                department = new Department();
            }

            return _dataShaper.ShapeData(department, fields);
        }

        public Department GetDepartmentById(Guid departmentId)
        {
            return FindByCondition(i => i.Id.Equals(departmentId)).FirstOrDefault();
        }

        public void UpdateDepartment(Department dbDepartment, Department department)
        {
            dbDepartment.Map(department);
            Update(dbDepartment);
        }

        private void SearchByName(ref IQueryable<Department> departments, string departmentName)
        {
            if (!departments.Any() || string.IsNullOrWhiteSpace(departmentName))
            {
                return;
            }

            departments = departments.Where(i => i.Name.ToLower().Contains(departmentName.Trim().ToLower()));
        }
    }
}