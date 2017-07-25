using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace WebApi
{
    class SqlServer
    {
        private static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Conexao"].ConnectionString;
            }
        }

        public static string Scalar(string strSQL)
        {
            Params arrParams = new Params();
            return Scalar(strSQL, arrParams);
        }

        public static string Scalar(string strSQL, Params arrParams)
        {
            SqlParameter[] sqlParams = arrParams.ToArray();
            SqlConnection myConnection = new SqlConnection(SqlServer.ConnectionString);
            SqlCommand myCmd = new SqlCommand(strSQL, myConnection);
            myCmd.CommandType = CommandType.Text;
            if (sqlParams.Length > 0)
            {
                for (int i = 0; i < sqlParams.Length; i++)
                {
                    myCmd.Parameters.Add(sqlParams[i]);
                }
            }
            myConnection.Open();
            string strScalarValue = myCmd.ExecuteScalar().ToString();
            myCmd.ExecuteNonQuery();
            myConnection.Close();
            return strScalarValue;
        }

        public static DataTable Recordset(string strSQL)
        {
            Params arrParams = new Params();
            return Recordset(strSQL, arrParams);
        }

        public static DataTable Recordset(string strSQL, Params arrParams)
        {

            SqlParameter[] sqlParams = arrParams.ToArray();
            SqlConnection myConnection = new SqlConnection(SqlServer.ConnectionString);
            SqlDataAdapter myAdapter = new SqlDataAdapter();
            myAdapter.SelectCommand = new SqlCommand(strSQL, myConnection);
            myAdapter.SelectCommand.CommandType = CommandType.Text;
            if (sqlParams.Length > 0)
            {
                for (int i = 0; i < sqlParams.Length; i++)
                {
                    myAdapter.SelectCommand.Parameters.Add(sqlParams[i]);
                }
            }
            DataTable dt = new DataTable();
            myAdapter.Fill(dt);
            return dt;
        }

        public static void Execute(string strSQL)
        {
            Params arrParams = new Params();
            int x = Execute(strSQL, arrParams, true);
        }

        public static void Execute(string strSQL, Params arrParams)
        {
            int x = Execute(strSQL, arrParams, true);
        }

        public static int Execute(string strSQL, Params arrParams, bool ReturnID)
        {
            strSQL = strSQL + "; SELECT SCOPE_IDENTITY() AS NewID;";
            SqlConnection myConnection = new SqlConnection(SqlServer.ConnectionString);
            SqlCommand myCmd = new SqlCommand(strSQL, myConnection);
            myCmd.CommandType = CommandType.Text;
            if (arrParams.Count > 0)
            {
                for (int i = 0; i < arrParams.Count; i++)
                {
                    myCmd.Parameters.Add(arrParams[i]);
                }
            }
            myConnection.Open();
            object message = myCmd.ExecuteScalar();
            myConnection.Close();

            int id = 0;
            Int32.TryParse(message.ToString(), out id);
            return id;
        }
    }

    class Params
    {
        private ArrayList _params;

        public Params()
        {
            _params = new ArrayList();
        }

        public void Add(string sName, object oValue)
        {
            Add(sName, oValue, false);
        }

        public void Add(string sName, object oValue, bool IsBinary)
        {
            if (IsBinary)
            {
                byte[] image = (byte[])oValue;
                _params.Add(new SqlParameter(sName, SqlDbType.Image, image.Length, ParameterDirection.Input, false, 0, 0, null, DataRowVersion.Current, image));
            }
            else
            {
                _params.Add(new SqlParameter(sName, oValue.ToString()));
            }
        }


        public SqlParameter[] ToArray()
        {
            return (SqlParameter[])_params.ToArray(typeof(SqlParameter));
        }

        public object this[int index]
        {
            get
            {
                if (index < 0 || index >= _params.Count)
                {
                    // handle bad index
                }
                object[] aObjs = _params.ToArray();
                return aObjs[index];
            }
        }

        public int Count
        {
            get
            {
                return _params.Count;
            }
        }
    }
}