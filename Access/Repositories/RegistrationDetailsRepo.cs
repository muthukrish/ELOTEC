using ELOTEC.Access.Interfaces;
using ELOTEC.Models;
using ELOTEC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ELOTEC.Infrastructure.Common;
using ELOTEC.Infrastructure.Constants;

namespace ELOTEC.Access.Repositories
{
    public class RegistrationDetailsRepo : DataBaseConnectionProvider, IRegistrationDetails
    {
        private readonly ResultObject _result;
        public RegistrationDetailsRepo()
        {
            _result = new ResultObject();
        }
        public async Task<ResultObject> UpdateRegistrationDetails(int UserId, int DeviceId, int ItemId, bool IsReg, string Axis)
        {
            try
            {
                //RegistrationDetailsVM RegistrationDetails = new RegistrationDetailsVM();
                using (var con = new SqlConnection(ConnectionString))
                {
                    await con.OpenAsync();
                    using (SqlCommand sqlCommand = new SqlCommand("sp_UpdateRegistrationDetails", con))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@userId", UserId);
                        sqlCommand.Parameters.AddWithValue("@deviceId", DeviceId);
                        sqlCommand.Parameters.AddWithValue("@itemId", ItemId);
                        sqlCommand.Parameters.AddWithValue("@IsReg", IsReg);
                        sqlCommand.Parameters.AddWithValue("@axis", Axis);
                        if (sqlCommand.ExecuteNonQuery() == 0)
                        {
                            _result[ResultKey.Success] = false;
                            _result[ResultKey.Message] = Message.Failed;
                        }
                        else
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;

                            using (SqlDataAdapter sda = new SqlDataAdapter("sp_GetDeviceLastUpdatedDetails", con))
                            {
                                DataSet dsDeviceDetails = new DataSet();
                                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                                sda.SelectCommand.Parameters.AddWithValue("@deviceId", DeviceId);
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
                                            DeviceLastUpdatedDetails.Add(objCP);
                                        }
                                        _result[ResultKey.LastUpdatedInfo] = DeviceLastUpdatedDetails;
                                    }
                                }
                            }
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

        public async Task<ResultObject> GetRegistrationHistory(int userId, int deviceId)
        {
            try
            {
                RegistrationDetailsVM RegistrationDetails = new RegistrationDetailsVM();
                using (var sqlConnection = new SqlConnection(ConnectionString))
                {
                    await sqlConnection.OpenAsync();
                    using (SqlDataAdapter dap = new SqlDataAdapter("sp_GetDeviceDetails", sqlConnection))
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
                            List<RegistrationVM> Registration = new List<RegistrationVM>();
                            foreach (DataRow x in dsRegistration.Tables[0].Rows)
                            {
                                RegistrationVM objCP = new RegistrationVM();
                                objCP.DeviceId = Convert.ToInt32(x["DeviceId"]);
                                objCP.DeviceName = Convert.ToString(x["Device"]);
                                objCP.UserId = Convert.ToInt32(x["UserId"]);
                                objCP.ItemId = Convert.ToInt32(x["ItemId"]);
                                objCP.ItemName = Convert.ToString(x["Item"]);
                                objCP.Axis = Convert.ToString(x["Axis"]);
                                objCP.IsRegistered = Convert.ToByte(x["IsRegistered"]);
                                objCP.IsActive = Convert.ToByte(x["IsActive"]);
                                Registration.Add(objCP);
                            }
                            _result[ResultKey.RegistrationDetails] = Registration;
                        }
                        else
                        {
                            _result[ResultKey.Success] = false;
                            _result[ResultKey.Message] = "User doesn't exist";
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
