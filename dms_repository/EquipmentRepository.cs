using DepartmentManagementSystem.Contracts;
using DepartmentManagementSystem.Entities;
using DepartmentManagementSystem.Entities.Helpers;
using DepartmentManagementSystem.Entities.Models;
using DepartmentManagementSystem.Entities.Parameters;
using DepartmentManagementSystem.Entities.ShapedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Repository
{
    public class EquipmentRepository: RepositoryBase<Equipment>, IEquipmentRepository
    {
        private readonly ISortHelper<Equipment> _sortHelper;

        private readonly IDataShaper<Equipment> _dataShaper;

        public EquipmentRepository(RepositoryContext repositoryContext, ISortHelper<Equipment> sortHelper, IDataShaper<Equipment> dataShaper) : base(repositoryContext)
        {
            _sortHelper = sortHelper;
            _dataShaper = dataShaper;
        }

        public void CreateEquipment(Equipment equipment)
        {
            equipment.TimeCreated = DateTime.Now;
            Create(equipment);
        }

        public void DeleteEquipment(Equipment equipment)
        {
            equipment.Deleted = true;
            equipment.TimeDeleted = DateTime.Now;
            Equipment dbEquipment = GetEquipmentById(equipment.Id);
            UpdateEquipment(dbEquipment, equipment);
        }

        public PagedList<ShapedEntity> GetAllEquipments(EquipmentParameters equipmentParameters)
        {
            var equipments = FindByCondition(equipment => !equipment.Deleted);

            SearchByName(ref equipments, equipmentParameters.Name);

            var sortedEquipments = _sortHelper.ApplySort(equipments, equipmentParameters.OrderBy);
            var shapedEquipments = _dataShaper.ShapeData(sortedEquipments, equipmentParameters.Fields).AsQueryable();

            return PagedList<ShapedEntity>.ToPagedList(shapedEquipments, equipmentParameters.PageNumber, equipmentParameters.PageSize);
        }

        public ShapedEntity GetEquipmentById(Guid equipmentId, string fields)
        {
            var equipment = FindByCondition(equipment => equipment.Id.Equals(equipmentId)).FirstOrDefault();

            if (equipment == null)
            {
                equipment = new Equipment();
            }

            return _dataShaper.ShapeData(equipment, fields);
        }

        public Equipment GetEquipmentById(Guid equipmentId)
        {
            return FindByCondition(i => i.Id.Equals(equipmentId)).FirstOrDefault();
        }

        public void UpdateEquipment(Equipment dbEquipment, Equipment equipment)
        {
            dbEquipment.Map(equipment);
            Update(dbEquipment);
        }

        private void SearchByName(ref IQueryable<Equipment> equipments, string equipmentName)
        {
            if (!equipments.Any() || string.IsNullOrWhiteSpace(equipmentName))
            {
                return;
            }

            equipments = equipments.Where(i => i.Name.ToLower().Contains(equipmentName.Trim().ToLower()));
        }
    }
}
