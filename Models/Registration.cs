using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELOTEC.Models
{

    public class RegistrationDetailsVM
    {
        public ICollection<RegistrationVM> Registration { get; set; }
        public ICollection<DeviceLastUpdatedDetailsVM> DeviceLastUpdatedDetails { get; set; }
    }

    public class RegistrationVM
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Axis { get; set; }
        public byte IsRegistered { get; set; }
        public byte IsActive { get; set; }
    }
    public class DeviceLastUpdatedDetailsVM
    {
        public string LastUpdatedUser { get; set; }
        public string Updated_Date { get; set; }
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }

    }

    public class Registration
    {
        public int UserId { get; set; }
        public int DeviceId { get; set; }
        public int ItemId { get; set; }
        public bool IsRegistered { get; set; }
        public string Axis { get; set; }
    }
    public class GetRegistered
    {
        public int UserId { get; set; }
        public int DeviceId { get; set; }
    }

}
