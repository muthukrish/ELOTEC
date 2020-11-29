using ELOTEC.Access.Interfaces;
using ELOTEC.Infrastructure.Common;
using ELOTEC.Infrastructure.Constants;
using ELOTEC.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ELOTEC.Access.Repositories
{
    public class DeviceSettingRepo : DataBaseConnectionProvider, IDeviceSetting
    {
        private readonly ResultObject _result;
        public DeviceSettingRepo()
        {
            this._result = new ResultObject();
        }


        public async Task<ResultObject> UpdateDeviceSetting(int userId, int deviceId, int radorCoverageVal, byte radorCoverageOnOff, int radorSensitivityLevelval, byte radorSensitivityOnOff, byte beepOnoff, byte RadorLEDOnoff)
        {
            try
            {

                using (var sqlcon = new NpgsqlConnection("Server = localhost; Username = postgres; Password = sa; Database = elocare;"))
                {
                    sqlcon.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("call sp_rl_updatedevicesettings(@mdid,@ble,@beep,@led,@radar," +
                                                                    "@rcstate,@rca,@rsstate,@rsl,@e1,@e2,@mic,@rmin,@rmax,@pmin,@pmax,@cmt,@mby)", sqlcon))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@mdid", deviceId);
                        command.Parameters.AddWithValue("@ble", true);
                        command.Parameters.AddWithValue("@beep", Convert.ToBoolean(beepOnoff));
                        command.Parameters.AddWithValue("@led", Convert.ToBoolean(RadorLEDOnoff));
                        command.Parameters.AddWithValue("@radar", true);
                        command.Parameters.AddWithValue("@rcstate", Convert.ToBoolean(radorCoverageOnOff));
                        command.Parameters.AddWithValue("@rca", radorCoverageVal == null ? (object)DBNull.Value : Convert.ToDouble(radorCoverageVal));
                        command.Parameters.AddWithValue("@rsstate", Convert.ToBoolean(radorSensitivityOnOff));
                        command.Parameters.AddWithValue("@rsl", radorSensitivityLevelval == null ? (object)DBNull.Value : Convert.ToDouble(radorSensitivityLevelval));
                        command.Parameters.AddWithValue("@e1", true);
                        command.Parameters.AddWithValue("@e2", true);
                        command.Parameters.AddWithValue("@mic", true);
                        command.Parameters.AddWithValue("@rmin", 6);
                        command.Parameters.AddWithValue("@rmax", 16);
                        command.Parameters.AddWithValue("@pmin", 72);
                        command.Parameters.AddWithValue("@pmax", 10);
                        command.Parameters.AddWithValue("@cmt", (object)DBNull.Value);
                        command.Parameters.AddWithValue("@mby", 1);
                        if (command.ExecuteNonQuery() == 0)
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
                    sqlcon.Close();
                }
                //using (var con = new SqlConnection(ConnectionString))
                //{
                //    await con.OpenAsync();
                //    using (SqlCommand cmd = new SqlCommand("sp_UpdateRadorSettings", con))
                //    {
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@UserId", userId);
                //        cmd.Parameters.AddWithValue("@deviceId", deviceId);
                //        cmd.Parameters.AddWithValue("@radorCoverageVal", radorCoverageVal);
                //        cmd.Parameters.AddWithValue("@radorCoverageOnOffStatus", radorCoverageOnOff);
                //        cmd.Parameters.AddWithValue("@radorSensitivityLevelval", radorSensitivityLevelval);
                //        cmd.Parameters.AddWithValue("@radorSensitivityOnOff", radorSensitivityOnOff);
                //        cmd.Parameters.AddWithValue("@beepOnoff", beepOnoff);
                //        cmd.Parameters.AddWithValue("@radorIndicatorStatus", RadorLEDOnoff);
                //        if (cmd.ExecuteNonQuery() == 0)
                //        {
                //            _result[ResultKey.Success] = false;
                //            _result[ResultKey.Message] = Message.Failed;
                //        }
                //        else
                //        {
                //            _result[ResultKey.Success] = true;
                //            _result[ResultKey.Message] = Message.Success;
                //        }
                //    }
                //    con.Close();
                //}
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ResultObject> GetDeviceSettingDetails(int userId, int deviceId)
        {
            try
            {
                DeviceSettingVM DeviceSetting = new DeviceSettingVM();
                using (var sqlcon = new NpgsqlConnection("Server = localhost; Username = postgres; Password = sa; Database = elocare;"))
                {
                    sqlcon.Open();
                    using (NpgsqlDataAdapter dap = new NpgsqlDataAdapter("fn_getmasterdevice", sqlcon))
                    {
                        DataSet dsRegistration = new DataSet();
                        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                        dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@mdid", deviceId));
                        dap.Fill(dsRegistration);
                        if (dsRegistration.Tables.Count > 0)
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;

                            List<DeviceSettingVM> DeviceSettings = new List<DeviceSettingVM>();
                            foreach (DataRow x in dsRegistration.Tables[0].Rows)
                            {
                                DeviceSettingVM objCP = new DeviceSettingVM();

                                //objCP.DeviceId = Convert.ToInt32(x.ItemArray[0]);
                                //objCP.DeviceName = Convert.ToString(x.ItemArray[1]);
                                //objCP.IsActive = Convert.ToByte(x.ItemArray[19]);
                                //objCP.RadorCoverageArea = Convert.ToDouble(x.ItemArray[9]);
                                //objCP.RadorCoverageStatus = Convert.ToByte(x.ItemArray[8]);
                                //objCP.RadorSensitivityLevel = Convert.ToDouble(x.ItemArray[11]);
                                //objCP.RadorSensitivityStatus = Convert.ToByte(x.ItemArray[10]);
                                //objCP.BeepStatus = Convert.ToByte(x.ItemArray[5]);
                                //objCP.RadorLEDIndicatorStatus = Convert.ToByte(x.ItemArray[6]);
                                //objCP.IPAddress = Convert.ToString(x.ItemArray[1]);


                                objCP.DeviceId = Convert.ToInt32(x["masterdeviceid"]);
                                objCP.DeviceName = Convert.ToString(x["name"]);
                                objCP.IsActive = Convert.ToByte(x["isactive"]);
                                objCP.RadorCoverageArea = Convert.ToDouble(x["rcavalue"]);
                                objCP.RadorCoverageStatus = Convert.ToByte(x["radarstate"]);
                                objCP.RadorSensitivityLevel = Convert.ToDouble(x["rslvalue"]);
                                objCP.RadorSensitivityStatus = Convert.ToByte(x["rslstate"]);
                                objCP.BeepStatus = Convert.ToByte(x["beepstate"]);
                                objCP.RadorLEDIndicatorStatus = x["ledstate"] != DBNull.Value ? Convert.ToByte(x["rcavalue"]) : Convert.ToByte(x["rcavalue"]);
                                objCP.IPAddress = Convert.ToString(x["ipaddress"]);


                                //objCP.DeviceId = Convert.ToInt32(x["DeviceId"]);
                                //objCP.DeviceName = Convert.ToString(x["DeviceName"]);
                                //objCP.IsActive = Convert.ToByte(x["IsActive"]);
                                //objCP.RadorCoverageArea = Convert.ToInt32(x["RadorCoverageArea"]);
                                //objCP.RadorCoverageStatus = Convert.ToByte(x["RadorCoverageStatus"]);
                                //objCP.RadorSensitivityLevel = Convert.ToInt32(x["RadorSensitivityLevel"]);
                                //objCP.RadorSensitivityStatus = Convert.ToByte(x["RadorSensitivityStatus"]);
                                //objCP.BeepStatus = Convert.ToByte(x["BeepStatus"]);
                                //objCP.RadorLEDIndicatorStatus = Convert.ToByte(x["RadorLEDIndicatorStatus"]);
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
                    sqlcon.Close();
                }
                //using (var sqlConnection = new SqlConnection(ConnectionString))
                //{
                //    await sqlConnection.OpenAsync();
                //    using (SqlDataAdapter dap = new SqlDataAdapter("sp_GetDeviceSettingDetails", sqlConnection))
                //    {
                //        DataSet dsRegistration = new DataSet();
                //        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                //        dap.SelectCommand.Parameters.AddWithValue("@UserId", userId);
                //        dap.SelectCommand.Parameters.AddWithValue("@deviceId", deviceId);
                //        dap.Fill(dsRegistration);
                //        if (dsRegistration.Tables.Count > 0)
                //        {
                //            _result[ResultKey.Success] = true;
                //            _result[ResultKey.Message] = Message.Success;
                //            List<DeviceSettingVM> DeviceSettings = new List<DeviceSettingVM>();
                //            foreach (DataRow x in dsRegistration.Tables[0].Rows)
                //            {
                //                DeviceSettingVM objCP = new DeviceSettingVM();
                //                objCP.DeviceId = Convert.ToInt32(x["DeviceId"]);
                //                objCP.DeviceName = Convert.ToString(x["DeviceName"]);
                //                objCP.IsActive = Convert.ToByte(x["IsActive"]);
                //                objCP.RadorCoverageArea = Convert.ToInt32(x["RadorCoverageArea"]);
                //                objCP.RadorCoverageStatus = Convert.ToByte(x["RadorCoverageStatus"]);
                //                objCP.RadorSensitivityLevel = Convert.ToInt32(x["RadorSensitivityLevel"]);
                //                objCP.RadorSensitivityStatus = Convert.ToByte(x["RadorSensitivityStatus"]);
                //                objCP.BeepStatus = Convert.ToByte(x["BeepStatus"]);
                //                objCP.RadorLEDIndicatorStatus = Convert.ToByte(x["RadorLEDIndicatorStatus"]);
                //                DeviceSettings.Add(objCP);
                //            }
                //            _result[ResultKey.DeviceSettingDetails] = DeviceSettings;
                //        }
                //        else
                //        {
                //            _result[ResultKey.Success] = false;
                //            _result[ResultKey.Message] = "something went wrong";
                //        }
                //    }
                //    sqlConnection.Close();
                //}
                return _result;
            }
            catch (Exception ex)
            {
                _result[ResultKey.Success] = false;
                _result[ResultKey.Message] = ex.Message;
                throw ex;
            }
        }
        public async Task<ResultObject> GetDeviceDetails_old(int userId, int deviceId)
        {
            try
            {

                using (var sqlcon = new NpgsqlConnection("Server = localhost; Username = postgres; Password = sa; Database = elocare;"))
                {
                    sqlcon.Open();
                    using (NpgsqlDataAdapter dap = new NpgsqlDataAdapter("fn_ro_registeredroomitems", sqlcon))
                    {
                        DataSet dsRegisteredroomitems = new DataSet();
                        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                        dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@rid", deviceId));
                        dap.Fill(dsRegisteredroomitems);
                        List<CustomItemVM> CustomItem = new List<CustomItemVM>();
                        if (dsRegisteredroomitems.Tables.Count > 0)
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            foreach (DataRow x in dsRegisteredroomitems.Tables[0].Rows)
                            {
                                CustomItemVM objCP = new CustomItemVM();
                                objCP.ItemId = Convert.ToInt32(x["riid"]);
                                objCP.ItemName = Convert.ToString(x["riname"]);
                                objCP.IsActive = Convert.ToByte(x["activeObj"]);
                                CustomItem.Add(objCP);
                            }
                            _result[ResultKey.CustomItemList] = CustomItem;
                        }
                        else
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            _result[ResultKey.CustomItemList] = CustomItem;
                        }
                    }
                    sqlcon.Close();
                }
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
        public async Task<ResultObject> GetDeviceDetails(int userId, int deviceId)
        {
            try
            {

                using (var sqlcon = new NpgsqlConnection("Server = localhost; Username = postgres; Password = sa; Database = elocare_new;"))
                {
                    sqlcon.Open();
                    using (NpgsqlDataAdapter dap = new NpgsqlDataAdapter("fn_ro_registeredroomitems", sqlcon))
                    {
                        DataSet dsRegisteredroomitems = new DataSet();
                        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                        dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@rid", deviceId));
                        dap.Fill(dsRegisteredroomitems);
                        List<CustomItemVM> CustomItem = new List<CustomItemVM>();
                        if (dsRegisteredroomitems.Tables.Count > 0)
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            foreach (DataRow x in dsRegisteredroomitems.Tables[0].Rows)
                            {
                                CustomItemVM objCP = new CustomItemVM();
                                objCP.ItemId = Convert.ToInt32(x["riid"]);
                                objCP.ItemName = Convert.ToString(x["riname"]);
                                objCP.IsActive = Convert.ToByte(x["activeObj"]);
                                CustomItem.Add(objCP);
                            }
                            _result[ResultKey.CustomItemList] = CustomItem;
                        }
                        else
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            _result[ResultKey.CustomItemList] = CustomItem;
                        }
                    }

                    using (NpgsqlDataAdapter dap = new NpgsqlDataAdapter("fn_GetDeviceInformation", sqlcon))
                    {
                        DataSet dsDeviceInformation = new DataSet();
                        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                        dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@uid", userId));
                        dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@did", deviceId));
                        dap.Fill(dsDeviceInformation);
                        List<DeviceInformationVM> DeviceInformationList = new List<DeviceInformationVM>();
                        if (dsDeviceInformation.Tables.Count > 0)
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            foreach (DataRow x in dsDeviceInformation.Tables[0].Rows)
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
                            using (NpgsqlDataAdapter sda = new NpgsqlDataAdapter("fn_GetDeviceLastUpdatedDetails", sqlcon))
                            {
                                DataSet dsDeviceDetails = new DataSet();
                                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                                sda.SelectCommand.Parameters.Add(new NpgsqlParameter("@did", deviceId));
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
                            using (NpgsqlDataAdapter sda = new NpgsqlDataAdapter("fn_defaultdevicesettings", sqlcon))
                            {
                                DataSet dsDeviceSetting = new DataSet();
                                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                                sda.SelectCommand.Parameters.AddWithValue("@rid", deviceId);
                                sda.Fill(dsDeviceSetting);
                                if (dsDeviceSetting.Tables.Count > 0)
                                {
                                    _result[ResultKey.Success] = true;
                                    _result[ResultKey.Message] = Message.Success;
                                    List<DeviceSettingVM> DeviceSettings = new List<DeviceSettingVM>();
                                    foreach (DataRow x in dsDeviceSetting.Tables[0].Rows)
                                    {
                                        DeviceSettingVM objCP = new DeviceSettingVM();
                                        objCP.DeviceId = Convert.ToInt32(x["masterdeviceid"]);
                                        objCP.DeviceName = Convert.ToString(x["name"]);
                                        objCP.IsActive = Convert.ToByte(x["isactive"]);
                                        objCP.RadorCoverageArea = x["rcavalue"] != DBNull.Value ? Convert.ToInt32(x["rcavalue"]) : 0;
                                        objCP.RadorCoverageStatus = x["rcastate"] != DBNull.Value ? Convert.ToByte(x["rcastate"]) : Convert.ToByte(0);
                                        objCP.RadorSensitivityLevel = x["rslvalue"] != DBNull.Value ? Convert.ToInt32(x["rslvalue"]) : 0;
                                        objCP.RadorSensitivityStatus = x["rslstate"] != DBNull.Value ? Convert.ToByte(x["rslstate"]) : Convert.ToByte(0); 
                                        objCP.BeepStatus = x["beepstate"] != DBNull.Value ? Convert.ToByte(x["beepstate"]) : Convert.ToByte(0);
                                        objCP.RadorLEDIndicatorStatus = x["ledstate"] != DBNull.Value ? Convert.ToByte(x["ledstate"]) : Convert.ToByte(0);
                                        objCP.IPAddress = Convert.ToString(x["ipaddress"]);
                                        //  objCP.SoftwareVersion = Convert.ToString(x["SoftwareVersion"]);
                                        objCP.SoftwareVersion = "1.1.10";
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
                    sqlcon.Close();
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
