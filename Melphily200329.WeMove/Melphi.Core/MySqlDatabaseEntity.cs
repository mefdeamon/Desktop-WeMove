using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace Melphi.Core
{
    public class MySqlDatabaseEntity
    {
        private string dbConnStr;
        private MySqlConnection dbConn = null;


        public MySqlDatabaseEntity(string connectionString)
        {
            dbConnStr = connectionString;
            dbConn = new MySqlConnection(dbConnStr);
        }

        ~MySqlDatabaseEntity()
        {
            Close();
        }

        public void Open()
        {
            dbConn.Open();
        }

        public void Close()
        {
            dbConn.Close();
        }

        public DataSet ExecuteSelect(string sql)
        {
            var mycom = dbConn.CreateCommand();
            mycom.CommandText = sql;
            MySqlDataAdapter adap = new MySqlDataAdapter(mycom);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            return ds;
        }
    }
}
