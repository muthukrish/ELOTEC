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
        public int RadorAdjustLevel { get; set; }
        public byte RadorAdjustStatus { get; set; }
        public int DbMeterAdjustLevel { get; set; }
        public byte DbMeterAdjustStatus { get; set; }
        public byte BeepStatus { get; set; }
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
