using DepartmentManagementSystem.Entities.Helpers;
using DepartmentManagementSystem.Entities.Models;
using DepartmentManagementSystem.Entities.Parameters;
using DepartmentManagementSystem.Entities.ShapedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Contracts
{
    public interface IEquipmentRepository: IRepositoryBase<Equipment>
    {
        PagedList<ShapedEntity> GetAllEquipments(EquipmentParameters equipmentParameters);
        ShapedEntity GetEquipmentById(Guid equipmentId, string fields);
        Equipment GetEquipmentById(Guid equipmentId);
        void CreateEquipment(Equipment equipment);
        void UpdateEquipment(Equipment dbEquipment, Equipment equipment);
        void DeleteEquipment(Equipment equipment);
    }
}
