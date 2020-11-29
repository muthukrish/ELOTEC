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
    public class ConfiguredRoomRepo : DataBaseConnectionProvider, IConfiguredRoom
    {
        private readonly ResultObject _result;
        public ConfiguredRoomRepo()
        {
            this._result = new ResultObject();
        }


        public async Task<ResultObject> GetDeviceSettingDetails_old(string filterStr, string fromDateVal, string todateVal, string deviceName, int userId, int Page, int Size)
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
                        dap.SelectCommand.Parameters.AddWithValue("@Page", Page);
                        dap.SelectCommand.Parameters.AddWithValue("@Size", Size);
                        dap.Fill(dsDeviceList);
                        List<ConfiguredRoomVM> DeviceList = new List<ConfiguredRoomVM>();
                        if (dsDeviceList.Tables.Count > 0)
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            foreach (DataRow x in dsDeviceList.Tables[0].Rows)
                            {
                                ConfiguredRoomVM objCP = new ConfiguredRoomVM();
                                objCP.DeviceId = Convert.ToInt32(x["DeviceId"]);
                                objCP.DeviceName = Convert.ToString(x["DeviceName"]);
                                objCP.LastUpdatedUser = Convert.ToString(x["LastUpdatedUser"]);
                                //objCP.Updated_Date = x["Updated_Date"] != DBNull.Value ? Convert.ToDateTime(x["Updated_Date"]) : (DateTime?)null;
                                objCP.Updated_Date = Convert.ToDateTime(x["Updated_Date"]).ToString("dd MMM yyyy");
                                objCP.IsRegistered = Convert.ToByte(x["RegisteredStatus"]);
                                DeviceList.Add(objCP);
                            }
                            _result[ResultKey.DeviceSettingDetails] = DeviceList;
                        }
                        else
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            _result[ResultKey.DeviceSettingDetails] = DeviceList;
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


        public async Task<ResultObject> GetDeviceSettingDetails(string filterStr, string fromDateVal, string todateVal, string deviceName, int userId, int Page, int Size)
        {
            try
            {
                List<ConfiguredRoomVM> DeviceList = new List<ConfiguredRoomVM>();
                using (var sqlcon = new NpgsqlConnection("Server = localhost; Username = postgres; Password = sa; Database = elocare;"))
                {
                    sqlcon.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("select * from fn_GetDeviceList(@filterStr,@Page,@Size)", sqlcon))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@filterStr", filterStr);
                        command.Parameters.AddWithValue("@Page", Page);
                        command.Parameters.AddWithValue("@Size", Size);
                        using (NpgsqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.HasRows && reader.Read())
                            {
                                DeviceList.Add(new ConfiguredRoomVM
                                {
                                    DeviceId = Convert.ToInt32(reader["DeviceId"]),
                                    DeviceName = Convert.ToString(reader["DeviceName"]),
                                    LastUpdatedUser = Convert.ToString(reader["LastUpdatedUser"]),
                                    Updated_Date = Convert.ToDateTime(reader["Updated_Date"]).ToString("dd MMM yyyy"),
                                    IsRegistered = Convert.ToByte(reader["RegisteredStatus"])
                                });
                            }
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            _result[ResultKey.DeviceSettingDetails] = DeviceList;
                        }
                    }


                    //using (NpgsqlDataAdapter dap = new NpgsqlDataAdapter("select * from fn_GetDeviceList(@filterStr,@Page,@Size)", sqlcon))
                    //{
                    //    DataSet dsDeviceList = new DataSet();
                    //    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    //    dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@filterStr", filterStr));
                    //    dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@Page", Page));
                    //    dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@Size", Size));

                    //    //  dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@onlyactive", true));
                    //    //dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@tesggt", 1));
                    //    //dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@filterStr", filterStr));
                    //    dap.Fill(dsDeviceList);
                    //    List<ConfiguredRoomVM> DeviceList = new List<ConfiguredRoomVM>();
                    //    if (dsDeviceList.Tables.Count > 0)
                    //    {
                    //        _result[ResultKey.Success] = true;
                    //        _result[ResultKey.Message] = Message.Success;
                    //        foreach (DataRow x in dsDeviceList.Tables[0].Rows)
                    //        {
                    //            ConfiguredRoomVM objCP = new ConfiguredRoomVM();
                    //            objCP.DeviceId = Convert.ToInt32(x["DeviceId"]);
                    //            objCP.DeviceName = Convert.ToString(x["DeviceName"]);
                    //            objCP.LastUpdatedUser = Convert.ToString(x["LastUpdatedUser"]);
                    //            //objCP.Updated_Date = x["Updated_Date"] != DBNull.Value ? Convert.ToDateTime(x["Updated_Date"]) : (DateTime?)null;
                    //            objCP.Updated_Date = Convert.ToDateTime(x["Updated_Date"]).ToString("dd MMM yyyy");
                    //            objCP.IsRegistered = Convert.ToByte(x["RegisteredStatus"]);
                    //            DeviceList.Add(objCP);
                    //        }
                    //        _result[ResultKey.DeviceSettingDetails] = DeviceList;
                    //    }
                    //    else
                    //    {
                    //        _result[ResultKey.Success] = true;
                    //        _result[ResultKey.Message] = Message.Success;
                    //        _result[ResultKey.DeviceSettingDetails] = DeviceList;
                    //    }
                    //}
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


        public async Task<ResultObject> GetUserList()
        {
            try
            {

                using (var sqlcon = new NpgsqlConnection("Server = localhost; Username = postgres; Password = sa; Database = elocare;"))
                {
                    sqlcon.Open();
                    using (NpgsqlDataAdapter dap = new NpgsqlDataAdapter("fn_users", sqlcon))
                    {
                        DataSet dsActiveUsers = new DataSet();
                        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                        dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@onlyactive", true));
                        dap.Fill(dsActiveUsers);
                        List<UserListVM> UserList = new List<UserListVM>();
                        if (dsActiveUsers.Tables[0].Rows.Count > 0)
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            foreach (DataRow x in dsActiveUsers.Tables[0].Rows)
                            {
                                UserListVM objCP = new UserListVM();
                                objCP.UserId = Convert.ToInt32(x.ItemArray[0]);
                                objCP.FirstName = Convert.ToString(x.ItemArray[1]);
                                UserList.Add(objCP);
                            }
                            _result[ResultKey.UserList] = UserList;
                        }
                        else
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            _result[ResultKey.UserList] = UserList;
                        }
                    }
                    sqlcon.Close();
                }




                //using (var sqlConnection = new SqlConnection(ConnectionString))
                //{
                //    await sqlConnection.OpenAsync();
                //    using (SqlDataAdapter dap = new SqlDataAdapter("sp_GetUserList", sqlConnection))
                //    {
                //        DataSet dsUserList = new DataSet();
                //        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                //        dap.Fill(dsUserList);
                //        List<UserListVM> UserList = new List<UserListVM>();
                //        if (dsUserList.Tables[0].Rows.Count > 0)
                //        {
                //            _result[ResultKey.Success] = true;
                //            _result[ResultKey.Message] = Message.Success;
                //            foreach (DataRow x in dsUserList.Tables[0].Rows)
                //            {
                //                UserListVM objCP = new UserListVM();
                //                objCP.UserId = Convert.ToInt32(x["UserId"]);
                //                objCP.FirstName = Convert.ToString(x["FirstName"]);
                //                UserList.Add(objCP);
                //            }
                //            _result[ResultKey.UserList] = UserList;
                //        }
                //        else
                //        {
                //            _result[ResultKey.Success] = true;
                //            _result[ResultKey.Message] = Message.Success;
                //            _result[ResultKey.UserList] = UserList;
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

    }
}
