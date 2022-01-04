using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Parameters
{
    public class ToolParameters: QueryStringParameters
    {
        public ToolParameters()
        {
            OrderBy = "name";
        }

        public string Name { get; set; }
    }
}
