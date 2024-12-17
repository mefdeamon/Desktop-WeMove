using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace MeiOrm
{
    public abstract class BaseSqlService
    {
        protected readonly string dbName = "db_name";
        public readonly SqlConnectionConfig sqlCon;
        protected readonly string _connectionString;
        protected BaseSqlService(string dbName, SqlConnectionConfig sqlCon)
        {
            this.dbName = dbName;
            this.sqlCon = sqlCon;
            this._connectionString = sqlCon.ToString();
        }

        public T QueryFirstOrDefault<T>(string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<T>(sql, param,transaction,commandTimeout, commandType);
            }
        }

        public  OperationResult Insert<T>(string sql,T entity) where T : class
        {
            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    var affectedRows = connection.Execute(sql, entity);

                    return new OperationResult
                    {
                        Success = affectedRows == 1,
                        Message = affectedRows == 1 ? "insert successful" : $"affected rows: {affectedRows}"
                    };
                }
            }
            catch (MySqlException mysqlEx)
            {
                return new OperationResult { Success = false, Message = $"mysql exception: {mysqlEx.Message}" };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = $"other exception: {ex.Message}" };
            }
        }
    }
}