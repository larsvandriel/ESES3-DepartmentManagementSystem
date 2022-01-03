namespace DepartmentManagementSystem.Entities.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public JobBreakDown JobBreakDown { get; set; }
        public EmployeeFunction RequiredFunction { get; set; }
    }
}