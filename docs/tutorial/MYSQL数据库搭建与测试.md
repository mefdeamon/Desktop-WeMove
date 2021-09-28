# MYSQL数据库搭建与测试

安装MYSQL 5.5版本

start server失败时，

1.先检查C盘的Program Files   Program Files（X86）    Windows   有没有 MYSQL文件，有就删除

2.检查我们使用的端口（例如3306）有没有被占用，如果有就把那个程序给暂停。（切换一个没有工作的端口）。





安装MYSQL-FONT可视化工具

登录到本地

新建数据库 wemove

新建用户信息表 user_info 及其相关字段

```mysql
CREATE TABLE `user_info` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(255) DEFAULT NULL COMMENT '用户名',
  `Email` varchar(255) NOT NULL DEFAULT '' COMMENT '邮箱',
  `Password` varchar(255) NOT NULL DEFAULT '' COMMENT '密码',
  `CreateDate` date DEFAULT '2020-07-16' COMMENT '创建日期',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='用户信息表';
```

新增一行数据（用于测试使用）

在WeMove项目添加Mysql.Data

添加数据库访问帮助类MySqlDatabaseHelper

```c#
 public class MySqlDatabaseHelper
    {
        private string dbConnStr;
        private MySqlConnection dbConn = null;


        public MySqlDatabaseHelper(string connectionString)
        {
            dbConnStr = connectionString;
            dbConn = new MySqlConnection(dbConnStr);
        }

        ~MySqlDatabaseHelper()
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
```

登录时，查询邮箱

```c#
await Task.Run(() =>
{

    string ConnectionString = "server = 127.0.0.1; userid = root; password = deamon; database = wemove; persistsecurityinfo = True;";
    MySqlDatabaseHelper mduser = new MySqlDatabaseHelper(ConnectionString);

    // 打开数据库
    mduser.Open();

    // sql 查询语句
    string sqlCmdStr = "select Email from user_info";

    var data = mduser.ExecuteSelect(sqlCmdStr);
    if (data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
    {
        var datt = data.Tables[0];
       
        if (datt.Rows[0]["Email"].ToString() == email)
        {
            App.Current.Dispatcher.Invoke(() => ServiceProvider.Get<SignViewModel>().CurrentView = new Views.Sign.SignInPassView());
        }
    }

    // 关闭数据库
    mduser.Close();

});

```











SQL操作资料

```c#
 string ConnectionString = "server = 127.0.0.1; userid = root; password = deamon; database = mduser; persistsecurityinfo = True;";
                        MySqlDatabaseHelper mduser = new MySqlDatabaseHelper(ConnectionString);

                        // 打开数据库
                        mduser.Open();

                        // sql 查询语句
                        string sqlCmdStr = null;

                        //// 新增记录
                        //sqlCmdStr = "insert into chipinfo_20200209(chipid, chiptype) values('5','S10')";
                        //mduser.ExecuteNonQuery(sqlCmdStr);

                        //// 删除记录
                        //sqlCmdStr = "delete from chipinfo_20200209 where chipid = 4";
                        //mduser.ExecuteNonQuery(sqlCmdStr);

                        //// 修改数据
                        //sqlCmdStr = "update chipinfo_20200209 set boardid = '666' where chipid = '5'";
                        //mduser.ExecuteNonQuery(sqlCmdStr);

                        // 查询数据
                        sqlCmdStr = "select * from chipinfo_20200209";
                        MySql.Data.MySqlClient.MySqlDataReader dataReader = mduser.ExecuteReader(sqlCmdStr);
                        while (dataReader.Read())
                        {
                            string str = dataReader["chiptype"].ToString();
                        }
                        dataReader.Close();

                        // 关闭数据库
                        mduser.Close();
```

