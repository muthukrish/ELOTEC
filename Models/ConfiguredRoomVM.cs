using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELOTEC.Models
{
    public class ConfiguredRoomVM
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string LastUpdatedUser { get; set; }
        public DateTime? Updated_Date { get; set; }
        public byte IsRegistered { get; set; }
    }
    public class UserListVM
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
    }
}
