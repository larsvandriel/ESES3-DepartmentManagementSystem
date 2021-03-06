namespace DepartmentManagementSystem.Entities.Models
{
    public class Task: IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public JobBreakDown JobBreakDown { get; set; }
        public EmployeeFunction RequiredFunction { get; set; }
        public List<Department> Departments { get; set; }
        public DateTime TimeCreated { get; set; }
        public bool Deleted { get; set; }
        public DateTime TimeDeleted { get; set; }
    }
}