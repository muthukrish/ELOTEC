using ELOTEC.Access.Interfaces;
using ELOTEC;
using ELOTEC.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ELOTEC.Infrastructure.Constants;
using Npgsql;
using NpgsqlTypes;

namespace ELOTEC.Access.Repositories
{
    public class LoginRepo : DataBaseConnectionProvider, ILogin
    {
        private readonly ResultObject _result;
        public LoginRepo()
        {
            _result = new ResultObject();
        }
        public async Task<ResultObject> LoginCheck(string userName, string password)
        {
            try
            {
                using (var sqlcon = new NpgsqlConnection(ConnectionString))
                {
                    sqlcon.Open();
                    using (var cmd = new NpgsqlCommand("fn_CheckLogin", sqlcon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue(":uname", userName);
                        //cmd.Parameters.AddWithValue(":passw", password);
                        cmd.Parameters.Add(new NpgsqlParameter("@uname", userName));
                        cmd.Parameters.Add(new NpgsqlParameter("@passw", password));
                        NpgsqlParameter outParm = new NpgsqlParameter("@UserIdVal", NpgsqlDbType.Integer)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outParm);
                        //cmd.Parameters.Add(new NpgsqlParameter("@userId", DbType.Int32));
                        //cmd.Parameters["@userId"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        if (!outParm.Value.Equals(DBNull.Value))
                        {
                            if (outParm.Value != null)
                            {
                                _result[ResultKey.Success] = true;
                                _result[ResultKey.Message] = Message.Success;
                                _result[ResultKey.UserId] = outParm.Value;
                            }
                            else {
                                _result[ResultKey.Success] = false;
                                _result[ResultKey.Message] = Message.Failed;
                            }
                        }
                        else {
                            _result[ResultKey.Success] = false;
                            _result[ResultKey.Message] = Message.Failed;
                        }
                    }
                    sqlcon.Close();
                }
                //using (var con = new SqlConnection(ConnectionString))
                //{
                //    await con.OpenAsync();

                //    con.Close();
                //}
                return _result;
            }
            catch (Exception)
            {
                throw;
            }


        }
    }
}
