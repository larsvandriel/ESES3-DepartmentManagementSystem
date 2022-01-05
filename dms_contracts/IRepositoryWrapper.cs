using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Contracts
{
    public interface IRepositoryWrapper
    {
        IDepartmentRepository Department { get; }
        IEmployeeFunctionRepository EmployeeFunction { get; }
        IEquipmentRepository Equipment { get; }
        IFacilityRepository Facility { get; }
        IRuleRepository Rule { get; }
        ITaskRepository Task { get; }
        IToolRepository Tool { get; }
        IUtilityRepository Utility { get; }

        void Save();
    }
}
