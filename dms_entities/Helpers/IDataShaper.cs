﻿using DepartmentManagementSystem.Entities.ShapedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Helpers
{
    public interface IDataShaper<T>
    {
        IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString);
        ShapedEntity ShapeData(T entity, string fieldString);
    }
}
