using ELOTEC.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELOTEC.Access.Interfaces
{
    interface ILogin
    {
        Task<ResultObject> LoginCheck(string userName, string password);
    }
}
