using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Models
{
    public class Report: IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Employee WrittenBy { get; set; }
        public DateTime TimeWritten { get; set; }
        public string FilePath { get; set; }
        public DateTime TimeEdited { get; set; }
        public bool Deleted { get; set; }
        public DateTime TimeDeleted { get; set; }
    }
}
