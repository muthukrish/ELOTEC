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
    public class CustomRegiatrationRepo : DataBaseConnectionProvider, ICustomRegiatration
    {
        private readonly ResultObject _result;
        public CustomRegiatrationRepo()
        {
            this._result = new ResultObject();
        }

        public async Task<ResultObject> GetCustomItemlist(int userId, int roomid)
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
                        dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@rid", roomid));
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


                //using (var sqlConnection = new SqlConnection(ConnectionString))
                //{
                //    await sqlConnection.OpenAsync();
                //    using (SqlDataAdapter dap = new SqlDataAdapter("sp_GetCustomItemList", sqlConnection))
                //    {
                //        DataSet dsCustomItem = new DataSet();
                //        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                //        dap.SelectCommand.Parameters.AddWithValue("@UserId", userId);
                //        dap.SelectCommand.Parameters.AddWithValue("@deviceId", deviceId);
                //        dap.Fill(dsCustomItem);
                //        List<CustomItemVM> CustomItem = new List<CustomItemVM>();
                //        if (dsCustomItem.Tables[0].Rows.Count > 0)
                //        {
                //            _result[ResultKey.Success] = true;
                //            _result[ResultKey.Message] = Message.Success;
                          
                //            foreach (DataRow x in dsCustomItem.Tables[0].Rows)
                //            {
                //                CustomItemVM objCP = new CustomItemVM();
                //                objCP.ItemId = Convert.ToInt32(x["ItemId"]);
                //                objCP.ItemName = Convert.ToString(x["ItemName"]);
                //                objCP.IsActive = Convert.ToByte(x["IsActive"]);
                //                //objCP.iscustom = Convert.ToByte(x["iscustom"]);
                //                //objCP.IsRegistered = Convert.ToByte(x["RegStatus"]);
                //                //objCP.RegistrationId = x["RegistrationId"] != DBNull.Value ? Convert.ToInt32(x["RegistrationId"]) : (int?)null;
                //                CustomItem.Add(objCP);
                //            }
                //            _result[ResultKey.CustomItemList] = CustomItem;
                //        }
                //        else
                //        {
                //            _result[ResultKey.Success] = true;
                //            _result[ResultKey.Message] = Message.Success;
                //            _result[ResultKey.CustomItemList] = CustomItem;
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


        public async Task<ResultObject> UpdateCustomItem(int userId, int deviceId, int itemId, byte RegStatus)
        {
            try
            {

                using (var sqlcon = new NpgsqlConnection("Server = localhost; Username = postgres; Password = sa; Database = elocare;"))
                {
                    sqlcon.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("call sp_updateCustomItem(@rid,@riid,@isactiveval)", sqlcon))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@rid", deviceId);
                        command.Parameters.AddWithValue("@riid", itemId);
                        command.Parameters.AddWithValue("@isactiveval", Convert.ToBoolean(RegStatus));
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
                //using (var sqlConnection = new SqlConnection(ConnectionString))
                //{
                //    await sqlConnection.OpenAsync();
                //    using (SqlCommand cmd = new SqlCommand("sp_UpdateCustomItem", sqlConnection))
                //    {
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@UserId", userId);
                //        cmd.Parameters.AddWithValue("@deviceId", deviceId);
                //        cmd.Parameters.AddWithValue("@itemId", itemId);
                //        cmd.Parameters.AddWithValue("@RegStatus", RegStatus);
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
                //    sqlConnection.Close();
                //}
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
