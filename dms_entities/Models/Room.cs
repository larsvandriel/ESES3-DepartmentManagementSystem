namespace DepartmentManagementSystem.Entities.Models
{
    public class Room: IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}