using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class LocationExtensions
    {
        public static void Map(this Location dbLocation, Location location)
        {
            dbLocation.LocationNumber = location.LocationNumber;
            dbLocation.Name = location.Name;
            dbLocation.Rooms = location.Rooms;
        }
    }
}
