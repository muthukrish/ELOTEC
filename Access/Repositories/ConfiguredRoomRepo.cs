﻿using ELOTEC.Access.Interfaces;
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
    public class ConfiguredRoomRepo: DataBaseConnectionProvider, IConfiguredRoom
    {
        private readonly ResultObject _result;
        public ConfiguredRoomRepo()
        {
            this._result = new ResultObject();
        }


        public async Task<ResultObject> GetDeviceSettingDetails(string filterStr, DateTime fromDateVal, DateTime todateVal, string deviceName, int userId)
        {
            try
            {
                ConfiguredRoomVM ConfiguredRoom = new ConfiguredRoomVM();
                using (var sqlConnection = new SqlConnection(ConnectionString))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlDataAdapter dap = new SqlDataAdapter("sp_GetDeviceList", sqlConnection))
                    {
                        DataSet dsDeviceList = new DataSet();
                        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                        dap.SelectCommand.Parameters.AddWithValue("@filterStr", filterStr);
                        dap.Fill(dsDeviceList);
                        if (dsDeviceList.Tables.Count > 0)
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            List<ConfiguredRoomVM> DeviceList = new List<ConfiguredRoomVM>();
                            foreach (DataRow x in dsDeviceList.Tables[0].Rows)
                            {
                                ConfiguredRoomVM objCP = new ConfiguredRoomVM();
                                objCP.DeviceId = Convert.ToInt32(x["DeviceId"]);
                                objCP.DeviceName = Convert.ToString(x["DeviceName"]);
                                objCP.LastUpdatedUser = Convert.ToString(x["LastUpdatedUser"]);
                                objCP.Updated_Date = x["Updated_Date"] != DBNull.Value ? Convert.ToDateTime(x["Updated_Date"]) : (DateTime?)null;
                                objCP.IsRegistered = Convert.ToByte(x["IsRegistered"]);
                                DeviceList.Add(objCP);
                            }
                            _result[ResultKey.DeviceSettingDetails] = DeviceList;
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