using ELOTEC.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELOTEC.Access.Interfaces
{
    interface IDeviceSetting
    {
        Task<ResultObject> UpdateDeviceSetting(int userId, int deviceId, int radorCoverageVal, byte radorCoverageOnOff, int radorSensitivityLevelval, byte radorSensitivityOnOff, byte beepOnoff, byte RadorLEDOnoff);
        Task<ResultObject> GetDeviceSettingDetails(int userId, int deviceId);
        Task<ResultObject> GetDeviceDetails(int userId, int deviceId);
    }
}
