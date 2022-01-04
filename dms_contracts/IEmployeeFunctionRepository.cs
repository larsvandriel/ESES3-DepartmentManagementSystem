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
    public interface IEmployeeFunctionRepository: IRepositoryBase<EmployeeFunction>
    {
        PagedList<ShapedEntity> GetAllEmployeeFunctions(EmployeeFunctionParameters employeeFunctionParameters);
        ShapedEntity GetEmployeeFunctionById(Guid employeeFunctionId, string fields);
        EmployeeFunction GetEmployeeFunctionById(Guid employeeFunctionId);
        void CreateEmployeeFunction(EmployeeFunction employeeFunction);
        void UpdateEmployeeFunction(EmployeeFunction dbEmployeeFunction, EmployeeFunction employeeFunction);
        void DeleteEmployeeFunction(EmployeeFunction employeeFunction);
    }
}
