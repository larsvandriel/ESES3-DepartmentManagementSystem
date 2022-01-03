using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationManagementSystem.Entities.ShapedEntities
{
    public class ShapedEntity
    {
        public Guid Id { get; set; }
        public Entity Entity { get; set; }

        public ShapedEntity()
        {
            Entity = new Entity();
        }
    }
}
