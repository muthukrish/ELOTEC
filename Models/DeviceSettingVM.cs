using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELOTEC.Models
{
    public class DeviceSettingVM
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public byte IsActive { get; set; }
        public double RadorCoverageArea { get; set; }
        public byte RadorCoverageStatus { get; set; }
        public double RadorSensitivityLevel { get; set; }
        public byte RadorSensitivityStatus { get; set; }
        public byte BeepStatus { get; set; }
        public byte RadorLEDIndicatorStatus { get; set; }
        public string IPAddress { get; set; }
        public string SoftwareVersion { get; set; }
    }
    public class DeviceInformationVM
    {
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public string Item { get; set; }
        public string Axis { get; set; }
        public byte IsRegistered { get; set; }
        public DateTime Updated_Date { get; set; }
        public string LastUpdatedUser { get; set; }
        public byte IsActive { get; set; }
    }
}
