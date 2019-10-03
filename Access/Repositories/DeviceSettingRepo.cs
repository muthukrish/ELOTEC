using ELOTEC.Access.Interfaces;
using ELOTEC.Infrastructure.Common;
using ELOTEC.Infrastructure.Constants;
using ELOTEC.Models;
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


        public async Task<ResultObject> GetDeviceSettingDetails(int userId, int deviceId)
        {
            try
            {
                DeviceSettingVM DeviceSetting = new DeviceSettingVM();
                using (var sqlConnection = new SqlConnection(ConnectionString))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlDataAdapter dap = new SqlDataAdapter("sp_GetDeviceSettingDetails", sqlConnection))
                    {
                        DataSet dsRegistration = new DataSet();
                        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                        dap.SelectCommand.Parameters.AddWithValue("@UserId", userId);
                        dap.SelectCommand.Parameters.AddWithValue("@deviceId", deviceId);
                        dap.Fill(dsRegistration);
                        if (dsRegistration.Tables.Count > 0)
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            List<DeviceSettingVM> DeviceSettings = new List<DeviceSettingVM>();
                            foreach (DataRow x in dsRegistration.Tables[0].Rows)
                            {
                                DeviceSettingVM objCP = new DeviceSettingVM();
                                objCP.DeviceId = Convert.ToInt32(x["DeviceId"]);
                                objCP.DeviceName = Convert.ToString(x["DeviceName"]);
                                objCP.IsActive = Convert.ToByte(x["IsActive"]);
                                objCP.RadorAdjustLevel = Convert.ToInt32(x["RadorAdjustLevel"]);
                                objCP.RadorAdjustStatus = Convert.ToByte(x["RadorAdjustStatus"]);
                                objCP.DbMeterAdjustLevel = Convert.ToInt32(x["DbMeterAdjustLevel"]);
                                objCP.DbMeterAdjustStatus = Convert.ToByte(x["DbMeterAdjustStatus"]);
                                objCP.BeepStatus = Convert.ToByte(x["BeepStatus"]);
                                DeviceSettings.Add(objCP);
                            }
                            _result[ResultKey.DeviceSettingDetails] = DeviceSettings;
                        }
                        else
                        {
                            _result[ResultKey.Success] = false;
                            _result[ResultKey.Message] = "something went wrong";
                        }
                    }
                    sqlConnection.Close();
                }
                return _result;
            }
            catch (Exception ex)
            {
                _result[ResultKey.Success] = false;
                _result[ResultKey.Message] = ex.Message;
                throw ex;
            }
        }

        public async Task<ResultObject> GetDeviceDetails(int userId, int deviceId)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(ConnectionString))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlDataAdapter dap = new SqlDataAdapter("sp_GetDeviceInformation", sqlConnection))
                    {
                        DataSet dsRegistration = new DataSet();
                        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                        dap.SelectCommand.Parameters.AddWithValue("@UserId", userId);
                        dap.SelectCommand.Parameters.AddWithValue("@deviceId", deviceId);
                        dap.Fill(dsRegistration);
                        if (dsRegistration.Tables.Count > 0)
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            List<DeviceInformationVM> DeviceInformationList = new List<DeviceInformationVM>();
                            foreach (DataRow x in dsRegistration.Tables[0].Rows)
                            {
                                DeviceInformationVM objCP = new DeviceInformationVM();
                                objCP.DeviceId = Convert.ToInt32(x["DeviceId"]);
                                objCP.DeviceName = Convert.ToString(x["DeviceName"]);
                                objCP.UserId = Convert.ToInt32(x["UserId"]);
                                objCP.ItemId = Convert.ToInt32(x["ItemId"]);
                                objCP.Item = Convert.ToString(x["Item"]);
                                objCP.Axis = Convert.ToString(x["Axis"]);
                                objCP.IsRegistered = Convert.ToByte(x["IsRegistered"]);
                                objCP.Updated_Date = Convert.ToDateTime(x["Updated_Date"]);
                                objCP.LastUpdatedUser = Convert.ToString(x["LastUpdatedUser"]);
                                objCP.IsActive = Convert.ToByte(x["IsActive"]);
                                DeviceInformationList.Add(objCP);
                            }
                            _result[ResultKey.DeviceSettingDetails] = DeviceInformationList;
                        }
                        else
                        {
                            _result[ResultKey.Success] = false;
                            _result[ResultKey.Message] = "something went wrong";
                        }
                    }
                    sqlConnection.Close();
                }
                return _result;
            }
            catch (Exception ex)
            {
                _result[ResultKey.Success] = false;
                _result[ResultKey.Message] = ex.Message;
                throw ex;
            }
        }
    }
}
