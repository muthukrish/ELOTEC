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


        public async Task<ResultObject> UpdateDeviceSetting(int userId, int deviceId, int radorCoverageVal, byte radorCoverageOnOff, int radorSensitivityLevelval, byte radorSensitivityOnOff, byte beepOnoff, byte RadorLEDOnoff) {
            try
            {
                using (var con = new SqlConnection(ConnectionString)) {
                    await con.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand("sp_UpdateRadorSettings", con)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@deviceId", deviceId);
                        cmd.Parameters.AddWithValue("@radorCoverageVal", radorCoverageVal);
                        cmd.Parameters.AddWithValue("@radorCoverageOnOffStatus", radorCoverageOnOff);
                        cmd.Parameters.AddWithValue("@radorSensitivityLevelval", radorSensitivityLevelval);
                        cmd.Parameters.AddWithValue("@radorSensitivityOnOff", radorSensitivityOnOff);
                        cmd.Parameters.AddWithValue("@beepOnoff", beepOnoff);
                        cmd.Parameters.AddWithValue("@radorIndicatorStatus", RadorLEDOnoff);
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
                                objCP.RadorCoverageArea = Convert.ToInt32(x["RadorCoverageArea"]);
                                objCP.RadorCoverageStatus = Convert.ToByte(x["RadorCoverageStatus"]);
                                objCP.RadorSensitivityLevel = Convert.ToInt32(x["RadorSensitivityLevel"]);
                                objCP.RadorSensitivityStatus = Convert.ToByte(x["RadorSensitivityStatus"]);
                                objCP.BeepStatus = Convert.ToByte(x["BeepStatus"]);
                                objCP.RadorLEDIndicatorStatus = Convert.ToByte(x["RadorLEDIndicatorStatus"]);
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
                        List<DeviceInformationVM> DeviceInformationList = new List<DeviceInformationVM>();
                        if (dsRegistration.Tables.Count > 0)
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            
                            foreach (DataRow x in dsRegistration.Tables[0].Rows)
                            {
                                DeviceInformationVM objCP = new DeviceInformationVM();
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
                            _result[ResultKey.RegistrationDetails] = DeviceInformationList;

                            using (SqlDataAdapter sda = new SqlDataAdapter("sp_GetDeviceLastUpdatedDetails", sqlConnection))
                            {
                                DataSet dsDeviceDetails = new DataSet();
                                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                                sda.SelectCommand.Parameters.AddWithValue("@deviceId", deviceId);
                                sda.Fill(dsDeviceDetails);
                                if (dsDeviceDetails.Tables.Count > 0)
                                {
                                    foreach (DataRow dr in dsDeviceDetails.Tables[0].Rows)
                                    {
                                        List<DeviceLastUpdatedDetailsVM> DeviceLastUpdatedDetails = new List<DeviceLastUpdatedDetailsVM>();
                                        foreach (DataRow x in dsDeviceDetails.Tables[0].Rows)
                                        {
                                            DeviceLastUpdatedDetailsVM objCP = new DeviceLastUpdatedDetailsVM();
                                            objCP.LastUpdatedUser = Convert.ToString(dr["LastUpdatedUser"]);
                                            objCP.Updated_Date = Convert.ToDateTime(dr["Updated_Date"]).ToString("dd MMM yyyy");
                                            objCP.DeviceId = Convert.ToInt32(dr["DeviceId"]);
                                            objCP.DeviceName = Convert.ToString(dr["DeviceName"]);
                                            DeviceLastUpdatedDetails.Add(objCP);
                                        }
                                        _result[ResultKey.LastUpdatedInfo] = DeviceLastUpdatedDetails;
                                    }
                                }
                            }
                            using (SqlDataAdapter sda = new SqlDataAdapter("sp_GetDeviceSettingDetails", sqlConnection))
                            {
                                DataSet dsDeviceSetting = new DataSet();
                                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                                sda.SelectCommand.Parameters.AddWithValue("@UserId", userId);
                                sda.SelectCommand.Parameters.AddWithValue("@deviceId", deviceId);
                                sda.Fill(dsDeviceSetting);
                                if (dsDeviceSetting.Tables.Count > 0)
                                {
                                    _result[ResultKey.Success] = true;
                                    _result[ResultKey.Message] = Message.Success;
                                    List<DeviceSettingVM> DeviceSettings = new List<DeviceSettingVM>();
                                    foreach (DataRow x in dsDeviceSetting.Tables[0].Rows)
                                    {
                                        DeviceSettingVM objCP = new DeviceSettingVM();
                                        objCP.DeviceId = Convert.ToInt32(x["DeviceId"]);
                                        objCP.DeviceName = Convert.ToString(x["DeviceName"]);
                                        objCP.IsActive = Convert.ToByte(x["IsActive"]);
                                        objCP.RadorCoverageArea = Convert.ToInt32(x["RadorCoverageArea"]);
                                        objCP.RadorCoverageStatus = Convert.ToByte(x["RadorCoverageStatus"]);
                                        objCP.RadorSensitivityLevel = Convert.ToInt32(x["RadorSensitivityLevel"]);
                                        objCP.RadorSensitivityStatus = Convert.ToByte(x["RadorSensitivityStatus"]);
                                        objCP.BeepStatus = Convert.ToByte(x["BeepStatus"]);
                                        objCP.RadorLEDIndicatorStatus = Convert.ToByte(x["RadorLEDIndicatorStatus"]);
                                        objCP.IPAddress = Convert.ToString(x["IpAddress"]);
                                        objCP.SoftwareVersion = Convert.ToString(x["SoftwareVersion"]);
                                        DeviceSettings.Add(objCP);
                                    }
                                    _result[ResultKey.DeviceSettingDetails] = DeviceSettings;
                                }
                            }
                        }
                        else
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            _result[ResultKey.DeviceSettingDetails] = DeviceInformationList;
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
