namespace MeiOrm
{
    /// <summary>
    /// 数据库连接信息配置
    /// </summary>
    public class SqlConnectionConfig
    {
        private string server = "127.0.0.1";
        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        private string userid = "root";
        public string Userid
        {
            get { return userid; }
            set { userid = value; }
        }

        private string port = "3306";

        public string Port
        {
            get { return port; }
            set { port = value; }
        }


        private string password = "meicreted@2024";
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string database = "db";

        public string Database
        {
            get { return database; }
            set { database = value; }
        }

        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"server = {server}; port={port}; userid = {userid}; password = {password}; database = {database};Charset=utf8; persistsecurityinfo = True;";
        }
    }
}