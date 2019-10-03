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
    public class DeviceSettingRepo:DataBaseConnectionProvider, IDeviceSetting
    {
        private readonly ResultObject _result;
        public DeviceSettingRepo()
        {
            this._result = new ResultObject();
        }


        public async Task<ResultObject> UpdateDeviceSetting(int userId, int deviceId, int radorlevelVal, byte radorOnOffStatus, int dbMeterLevelval, byte dbmeterOnOff, byte beepOnoff) {
            try
            {
                using (var con = new SqlConnection(ConnectionString)) {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateRadorSettings", con)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@deviceId", deviceId);
                        cmd.Parameters.AddWithValue("@radorlevelVal", radorlevelVal);
                        cmd.Parameters.AddWithValue("@radorOnOffStatus", radorOnOffStatus);
                        cmd.Parameters.AddWithValue("@dbMeterLevelval", dbMeterLevelval);
                        cmd.Parameters.AddWithValue("@dbmeterOnOff", dbmeterOnOff);
                        cmd.Parameters.AddWithValue("@beepOnoff", beepOnoff);
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
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}
