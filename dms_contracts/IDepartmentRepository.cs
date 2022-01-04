using DepartmentManagementSystem.Entities.Helpers;
using DepartmentManagementSystem.Entities.Models;
using DepartmentManagementSystem.Entities.Parameters;
using DepartmentManagementSystem.Entities.ShapedEntities;

namespace DepartmentManagementSystem.Contracts
{
    public interface IDepartmentRepository: IRepositoryBase<Department>
    {
        PagedList<ShapedEntity> GetAllDepartments(DepartmentParameters departmentParameters);
        ShapedEntity GetDepartmentById(Guid department, string fields);
        Department GetDepartmentById(Guid departementId);
        void CreateDepartment(Department department);
        void UpdateDepartment(Department dbDepartment, Department department);
        void DeleteDepartment(Department department);
    }
}