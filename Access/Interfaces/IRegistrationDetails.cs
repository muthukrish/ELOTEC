using ELOTEC.Infrastructure.Common;
using ELOTEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELOTEC.Access.Interfaces
{
    interface IRegistrationDetails
    {
        Task<ResultObject> UpdateRegistrationDetails(int userId, int deviceId, int itemId, bool IsReg, string access, int RadorlevelVal, byte radorOnOffStatus, int dbMeterLevelval, byte dbmeterOnOff, byte beepOnoff);
        Task<ResultObject> GetRegistrationHistory(int userId, int deviceId);
    }
}
