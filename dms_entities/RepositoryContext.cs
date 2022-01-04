using DepartmentManagementSystem.Entities.Configurations;
using DepartmentManagementSystem.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Task = DepartmentManagementSystem.Entities.Models.Task;

namespace DepartmentManagementSystem.Entities
{
    public class RepositoryContext: DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Archive> Archives { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeFunction> EmployeeFunctions { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<FinanceController> FinanceControllers { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<JobBreakDown> JobBreakDowns { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Tool> Tools { get; set; }
        public DbSet<Utility> Utilities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
            modelBuilder.ApplyConfiguration(new ArchiveConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeFunctionConfiguration());
            modelBuilder.ApplyConfiguration(new EquipmentConfiguration());
            modelBuilder.ApplyConfiguration(new FacilityConfiguration());
            modelBuilder.ApplyConfiguration(new FinanceControllerConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryConfiguration());
            modelBuilder.ApplyConfiguration(new JobBreakDownConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new ReportConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
            modelBuilder.ApplyConfiguration(new RuleConfiguration());
            modelBuilder.ApplyConfiguration(new StepConfiguration());
            modelBuilder.ApplyConfiguration(new TaskConfiguration());
            modelBuilder.ApplyConfiguration(new ToolConfiguration());
            modelBuilder.ApplyConfiguration(new UtilityConfiguration());
        }
    }
}