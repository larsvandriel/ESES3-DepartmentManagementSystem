using DepartmentManagementSystem.Contracts;
using DepartmentManagementSystem.Entities;
using DepartmentManagementSystem.Entities.Helpers;
using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = DepartmentManagementSystem.Entities.Models.Task;

namespace DepartmentManagementSystem.Repository
{
    public class RepositoryWrapper: IRepositoryWrapper
    {
        private RepositoryContext _repoContext;

        private IDepartmentRepository _department;
        private ISortHelper<Department> _departmentSortHelper;
        private IDataShaper<Department> _departmentDataShaper;

        private IEmployeeFunctionRepository _employeeFunction;
        private ISortHelper<EmployeeFunction> _employeeSortHelper;
        private IDataShaper<EmployeeFunction> _employeeDataShaper;

        private IEquipmentRepository _equipment;
        private ISortHelper<Equipment> _equipmentSortHelper;
        private IDataShaper<Equipment> _equipmentDataShaper;

        private IFacilityRepository _facility;
        private ISortHelper<Facility> _facilitySortHelper;
        private IDataShaper<Facility> _facitlityDataShaper;

        private IRuleRepository _rule;
        private ISortHelper<Rule> _ruleSortHelper;
        private IDataShaper<Rule> _ruleDataShaper;

        private ITaskRepository _task;
        private ISortHelper<Task> _taskSortHelper;
        private IDataShaper<Task> _taskDataShaper;

        private IToolRepository _tool;
        private ISortHelper<Tool> _toolSortHelper;
        private IDataShaper<Tool> _toolDataShaper;

        private IUtilityRepository _utility;
        private ISortHelper<Utility> _utilitySortHelper;
        private IDataShaper<Utility> _utilityDataShaper;

        public IDepartmentRepository Department
        {
            get
            {
                if (_department == null)
                {
                    _department = new DepartmentRepository(_repoContext, _departmentSortHelper, _departmentDataShaper);
                }

                return _department;
            }
        }

        public IEmployeeFunctionRepository EmployeeFunction
        {
            get
            {
                if (_employeeFunction == null)
                {
                    _employeeFunction = new EmployeeFunctionRepository(_repoContext, _employeeSortHelper, _employeeDataShaper);
                }

                return _employeeFunction;
            }
        }

        public IEquipmentRepository Equipment
        {
            get
            {
                if (_equipment == null)
                {
                    _equipment = new EquipmentRepository(_repoContext, _equipmentSortHelper, _equipmentDataShaper);
                }

                return _equipment;
            }
        }

        public IFacilityRepository Facility
        {
            get
            {
                if (_facility == null)
                {
                    _facility = new FacilityRepository(_repoContext, _facilitySortHelper, _facitlityDataShaper);
                }

                return _facility;
            }
        }

        public IRuleRepository Rule
        {
            get
            {
                if (_rule == null)
                {
                    _rule = new RuleRepository(_repoContext, _ruleSortHelper, _ruleDataShaper);
                }

                return _rule;
            }
        }

        public ITaskRepository Task
        {
            get
            {
                if (_task == null)
                {
                    _task = new TaskRepository(_repoContext, _taskSortHelper, _taskDataShaper);
                }

                return _task;
            }
        }

        public IToolRepository Tool
        {
            get
            {
                if (_tool == null)
                {
                    _tool = new ToolRepository(_repoContext, _toolSortHelper, _toolDataShaper);
                }

                return _tool;
            }
        }

        public IUtilityRepository Utility
        {
            get
            {
                if (_utility == null)
                {
                    _utility = new UtilityRepository(_repoContext, _utilitySortHelper, _utilityDataShaper);
                }

                return _utility;
            }
        }

        public RepositoryWrapper(RepositoryContext repositoryContext, ISortHelper<Department> departmentSortHelper, IDataShaper<Department> departmentDataShaper, ISortHelper<EmployeeFunction> employeeFunctionSortHelper, IDataShaper<EmployeeFunction> employeeFunctionDataShaper, ISortHelper<Equipment> equipmentSortHelper, IDataShaper<Equipment> equipmentDataShaper, ISortHelper<Facility> facilitySortHelper, IDataShaper<Product> productDataShaper)
        {
            _repoContext = repositoryContext;
            _departmentSortHelper = departmentSortHelper;
            _departmentDataShaper = departmentDataShaper;
            _employeeSortHelper = employeeFunctionSortHelper;
            _employeeDataShaper = employeeFunctionDataShaper;
            _equipmentSortHelper = equipmentSortHelper;
            _equipmentDataShaper = equipmentDataShaper;
            _facilitySortHelper = facilitySortHelper;
            _facitlityDataShaper = productDataShaper;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
