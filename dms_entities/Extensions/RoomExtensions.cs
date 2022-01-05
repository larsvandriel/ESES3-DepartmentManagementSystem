using DepartmentManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DepartmentManagementSystem.Entities.Extensions
{
    public static class RoomExtensions
    {
        public static void Map(this Room dbRoom, Room room)
        {
            dbRoom.Name = room.Name;
        }
    }
}
