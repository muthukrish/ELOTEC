using ELOTEC.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELOTEC.Access.Interfaces
{
    interface IDevice
    {
        Task<ResultObject> UpdateRoomNo(int userId, int deviceId,string roomName);
    }
}
