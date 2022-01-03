using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Models
{
    public class Step: IEntity
    {
        public Guid Id { get; set; }
        public Step MainStep { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

    }
}
