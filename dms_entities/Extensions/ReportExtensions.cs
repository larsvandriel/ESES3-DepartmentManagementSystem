using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class ReportExtensions
    {
        public static void Map(this Report dbReport, Report report)
        {
            dbReport.Name = report.Name;
            dbReport.Title = report.Title;
            dbReport.Content = report.Content;
            dbReport.WrittenBy = report.WrittenBy;
            dbReport.TimeWritten = report.TimeWritten;
            dbReport.FilePath = report.FilePath;
            dbReport.TimeEdited = report.TimeEdited;
            dbReport.Deleted = report.Deleted;
            dbReport.TimeDeleted = report.TimeDeleted;
        }
    }
}
