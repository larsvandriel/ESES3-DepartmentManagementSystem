using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class ArchiveExtensions
    {
        public static void Map(this Archive dbArchive, Archive archive)
        {
            dbArchive.Name = archive.Name;
            dbArchive.Reports = archive.Reports;
            dbArchive.TimeCreated = archive.TimeCreated;
            dbArchive.Deleted = archive.Deleted;
            dbArchive.TimeDeleted = archive.TimeDeleted;
        }
    }
}
