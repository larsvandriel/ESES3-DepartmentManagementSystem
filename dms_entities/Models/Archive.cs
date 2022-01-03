using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Models
{
    public class Archive
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Report> Reports { get; set; }
        public DateTime TimeCreated { get; set; }
        public bool Deleted { get; set; }
        public DateTime TimeDeleted { get; set; }
    }
}
