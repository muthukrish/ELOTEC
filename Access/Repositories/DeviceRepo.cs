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
    public class DeviceRepo : DataBaseConnectionProvider, IDevice
    {
        private readonly ResultObject _result;
        public DeviceRepo()
        {
            _result = new ResultObject();
        }

        public async Task<ResultObject> UpdateRoomNo(int userId, int deviceId, string roomName, string pfix, int roomNo)
        {
            try
            {
                //using (var con = new SqlConnection(ConnectionString))
                //{
                //    await con.OpenAsync();
                //    using (SqlCommand cmd = new SqlCommand("sp_UpdateRoomNo", con))
                //    {
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@userId", userId);
                //        cmd.Parameters.AddWithValue("@deviceId", deviceId);
                //        cmd.Parameters.AddWithValue("@roomName", roomName);
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


                using (var sqlcon = new NpgsqlConnection("Server = localhost; Username = postgres; Password = sa; Database = elocare;"))
                {
                    using (NpgsqlCommand command = new NpgsqlCommand("call sp_saveroom(@rid,@pfix,@rno,@rname)", sqlcon))
                    {
                        sqlcon.Open();
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@rid", deviceId);
                        command.Parameters.AddWithValue("@pfix", pfix);
                        command.Parameters.AddWithValue("@rno", roomNo);
                        command.Parameters.AddWithValue("@rname", roomName);

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
                        sqlcon.Close();
                    }
                }
                return _result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<ResultObject> DeviceRegRoomDetails(string bleid)
        {
            try
            {

                using (var sqlcon = new NpgsqlConnection("Server = localhost; Username = postgres; Password = sa; Database = elocare;"))
                {
                    sqlcon.Open();
                    using (NpgsqlDataAdapter dap = new NpgsqlDataAdapter("fn_checkdeviceexist", sqlcon))
                    {
                        DataSet dsCheckRoomExist = new DataSet();
                        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                        dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@blid", bleid));
                        dap.Fill(dsCheckRoomExist);

                        if (dsCheckRoomExist.Tables[0].Rows.Count > 0)
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            _result[ResultKey.License] = 1;
                            _result[ResultKey.DeviceId] = dsCheckRoomExist.Tables[0].Rows[0]["masterdeviceid"];
                            using (NpgsqlDataAdapter sda = new NpgsqlDataAdapter("fn_getregisteredroomfor_device", sqlcon))
                            {
                                DataSet dsDeviceRoomDeails = new DataSet();
                                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                                sda.SelectCommand.Parameters.Add(new NpgsqlParameter("@blid", bleid));
                                sda.Fill(dsDeviceRoomDeails);
                                if (dsDeviceRoomDeails.Tables.Count > 0)
                                {

                                    List<RoomVM> DeviceRegRoom = new List<RoomVM>();
                                    foreach (DataRow x in dsDeviceRoomDeails.Tables[0].Rows)
                                    {
                                        RoomVM objCP = new RoomVM();
                                        objCP.Roomid = Convert.ToInt32(x["roomid"]);
                                        objCP.Roomno = Convert.ToInt32(x["roomno"]);
                                        objCP.Name = Convert.ToString(x["name"]);
                                        DeviceRegRoom.Add(objCP);
                                    }
                                    _result[ResultKey.DeviceRegisteredRoom] = DeviceRegRoom;

                                }
                            }
                            using (NpgsqlDataAdapter sda = new NpgsqlDataAdapter("fn_getunregisteredroomlist", sqlcon))
                            {
                                DataSet dsUnRegRoomList = new DataSet();
                                sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                                sda.Fill(dsUnRegRoomList);
                                if (dsUnRegRoomList.Tables.Count > 0)
                                {
                                    List<RoomVM> UnRegRoomList = new List<RoomVM>();
                                    foreach (DataRow x in dsUnRegRoomList.Tables[0].Rows)
                                    {
                                        RoomVM objUnRegRoom = new RoomVM();
                                        objUnRegRoom.Roomid = Convert.ToInt32(x["roomid"]);
                                        objUnRegRoom.Roomno = Convert.ToInt32(x["roomno"]);
                                        objUnRegRoom.Name = Convert.ToString(x["name"]);
                                        UnRegRoomList.Add(objUnRegRoom);
                                    }
                                    _result[ResultKey.UnRegisteredRoomList] = UnRegRoomList;

                                }
                            }
                        }
                        else
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Failed;
                            _result[ResultKey.License] = 0;
                            _result[ResultKey.DeviceId] = 0;
                            List<RoomVM> DeviceRegRoom = new List<RoomVM>();
                            List<RoomVM> UnRegRoomList = new List<RoomVM>();
                            _result[ResultKey.DeviceRegisteredRoom] = DeviceRegRoom;
                            _result[ResultKey.UnRegisteredRoomList] = UnRegRoomList;
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


        public async Task<ResultObject> RegisterRoom(int userId, int deviceId, int roomId)
        {
            try
            {

                using (var sqlcon = new NpgsqlConnection("Server = localhost; Username = postgres; Password = sa; Database = elocare;"))
                {
                    sqlcon.Open();
                    using (NpgsqlDataAdapter dap = new NpgsqlDataAdapter("fn_checkroomregisteredornot", sqlcon))
                    {
                        DataSet dsCheckRoomRegistered = new DataSet();
                        dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                        dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@rid", roomId));
                        dap.SelectCommand.Parameters.Add(new NpgsqlParameter("@did", deviceId));
                        dap.Fill(dsCheckRoomRegistered);
                        if (dsCheckRoomRegistered.Tables[0].Rows.Count == 0)
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Success;
                            using (NpgsqlCommand command = new NpgsqlCommand("call sp_registerroomtodevice(@did,@rid)", sqlcon))
                            {
                                command.CommandType = CommandType.Text;
                                command.Parameters.AddWithValue("@did", deviceId);
                                command.Parameters.AddWithValue("@rid", roomId);
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

                        }
                        else
                        {
                            _result[ResultKey.Success] = true;
                            _result[ResultKey.Message] = Message.Failed;
                            _result[ResultKey.RegistrationDetails] = "Room Already Registered";
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


        public async Task<ResultObject> UnRegisterRoom(int userId, int deviceId, int roomId)
        {
            try
            {

                using (var sqlcon = new NpgsqlConnection("Server = localhost; Username = postgres; Password = sa; Database = elocare;"))
                {
                    sqlcon.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand("call sp_registerroomtodevice(@did,@rid)", sqlcon))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.AddWithValue("@did", deviceId);
                        command.Parameters.AddWithValue("@rid", roomId);
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
                    //_result[ResultKey.License] = 1;
                    //_result[ResultKey.DeviceId] = deviceId;
                    //List<RoomVM> DeviceRegRoom = new List<RoomVM>();
                    //_result[ResultKey.DeviceRegisteredRoom] = DeviceRegRoom;
                    //using (NpgsqlDataAdapter sda = new NpgsqlDataAdapter("fn_getunregisteredroomlist", sqlcon))
                    //{
                    //    DataSet dsUnRegRoomList = new DataSet();
                    //    sda.SelectCommand.CommandType = CommandType.StoredProcedure;
                    //    sda.Fill(dsUnRegRoomList);
                    //    if (dsUnRegRoomList.Tables.Count > 0)
                    //    {
                    //        List<RoomVM> UnRegRoomList = new List<RoomVM>();
                    //        foreach (DataRow x in dsUnRegRoomList.Tables[0].Rows)
                    //        {
                    //            RoomVM objUnRegRoom = new RoomVM();
                    //            objUnRegRoom.Roomid = Convert.ToInt32(x["roomid"]);
                    //            objUnRegRoom.Roomno = Convert.ToInt32(x["roomno"]);
                    //            objUnRegRoom.Name = Convert.ToString(x["name"]);
                    //            UnRegRoomList.Add(objUnRegRoom);
                    //        }
                    //        _result[ResultKey.UnRegisteredRoomList] = UnRegRoomList;

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
    }
}
