using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Models
{
    public class JobBreakDown
    {
        public Guid Id { get; set; }
        public Task Task { get; set; }
        public List<Tool> RequiredTools { get; set; }
        public List<Equipment> RequiredEquipment { get; set; }
        public List<Step> Steps { get; set; }

    }
}
