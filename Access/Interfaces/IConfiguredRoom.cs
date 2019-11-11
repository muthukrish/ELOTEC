using ELOTEC.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELOTEC.Access.Interfaces
{
    interface IConfiguredRoom
    {
        Task<ResultObject> GetDeviceSettingDetails(string filterStr, string fromDateVal, string todateVal, string deviceName, int userId, int Page, int Size);
        Task<ResultObject> GetUserList();

    }
}
