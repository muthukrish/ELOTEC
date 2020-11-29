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
        Task<ResultObject> UpdateRegistrationDetails(int UserId, int DeviceId, int ItemId, bool IsReg, string Axis,int roomid);
        Task<ResultObject> GetRegistrationHistory(int userId, int deviceId);
    }
}
