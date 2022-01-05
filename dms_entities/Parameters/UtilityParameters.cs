using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Parameters
{
    public class UtilityParameters: QueryStringParameters
    {
        public UtilityParameters()
        {
            OrderBy = "name";
        }

        public string Name { get; set; }
    }
}
