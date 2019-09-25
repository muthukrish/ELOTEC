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
                using (var con = new SqlConnection(ConnectionString))
                {
                    await con.OpenAsync();
                    using (var cmd = new SqlCommand("sp_CheckLogin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@userName", userName);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.Add(new SqlParameter("@userId", SqlDbType.Int));
                        cmd.Parameters["@userId"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        if (cmd.Parameters["@userId"].Value == null)
                        {
                            _result[ResultKey.Success] = false;
                            _result[ResultKey.Message] = Message.Failed;
                        }
                        else
                        {
                            if (!cmd.Parameters["@userId"].Value.Equals(DBNull.Value))
                            {
                                _result[ResultKey.Success] = true;
                                _result[ResultKey.Message] = Message.Success;
                                _result[ResultKey.UserId] = cmd.Parameters["@userId"].Value;
                            }
                            else
                            {
                                _result[ResultKey.Success] = false;
                                _result[ResultKey.Message] = Message.Failed;
                            }
                        }
                    }
                    con.Close();
                }
                return _result;
            }
            catch (Exception)
            {
                throw;
            }


        }
    }
}
