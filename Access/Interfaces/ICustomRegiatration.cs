using ELOTEC.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELOTEC.Access.Interfaces
{
    interface ICustomRegiatration
    {
        Task<ResultObject> GetCustomItemlist(int userId, int roomid);

        Task<ResultObject> UpdateCustomItem(int userId, int deviceId, int itemId, byte RegStatus);
    }
}
