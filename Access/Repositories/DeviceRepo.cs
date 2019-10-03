using ELOTEC.Access.Interfaces;
using ELOTEC.Infrastructure.Common;
using ELOTEC.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ELOTEC.Access.Repositories
{
    public class DeviceRepo : DataBaseConnectionProvider, IDevice
    {
        private readonly ResultObject _result;
        public DeviceRepo()
        {
            _result = new ResultObject();
        }

        public async Task<ResultObject> UpdateRoomNo(int userId, int deviceId, string roomName)
        {
            try
            {
                using (var con = new SqlConnection(ConnectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateRoomNo", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@deviceId", deviceId);
                        cmd.Parameters.AddWithValue("@roomName", roomName);
                        if (cmd.ExecuteNonQuery() == 0)
                        {
                            _result[ResultKey.Success] = false;
                            _result[ResultKey.Message] = Message.Failed;
                        }
                        else
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                        }

                    }
                    con.Close();
                }
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
