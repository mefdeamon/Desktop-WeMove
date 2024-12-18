

# 更新数据库到MYSQL8.0

更新后，原来的`MySql.Data`库文件无法正常`MYSQL8.0`+版本的服务进行登录验证。因此需要更新相关的库。

本文采用`MySql.Data 9.1.0`版本进行使用，同时使用`Dapper`简化数据库操作。

为了模块封装，数据库层，同样封装到库中。



## 面向Dapper的简单封装

正常情况下，Dapper已经是非常便捷的**ORM**了，不需要额外的封装。以下封装主要是为了将连接字符串进行封装。

### 1.创建类库 

- 库名：MeiOrm 
- 框架：.NET Standard 2.0

### 2.添加引用

- Dapper 2.1.35
- MySql.Data 9.1.0

### 3.添加数据库连接配置类

`SqlConnectionConfig.cs`

```c#
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
```

### 4.添加数据库操作结果类

`OperationResult.cs`

```c#
namespace MeiOrm
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
```

###  5.添加数据库访问基础类

`BaseSqlService.cs`

```C#
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
```



## 创建WebApi访问数据库

为了让客户端（WPF）不直接访问数据库，通过访问服务器API服务实现数据的查询和插入。

优点：

- 数据库访问放在内网会更加安全。数据库一般在服务器，或者服务器内网，而客户端的网络位置不确定。
- 客户端的类库引用变少
- 封装后，可以统一接口，对不同的应用（桌面应用、移动端APP、网站、小程序...）



### 1. 创建ASP.NET Core Web API 

- 名称：Melphi.WebApi 
- 框架：.NET 6.0
- 身份验证类型：无
- 配置HTTPS：取消
- 启用Dockers：取消
- 启用 OpenAPI支持：取消
- 使用控制器：取消
- 不使用顶级语句：取消（看习惯）

创建完成后，文件夹目录

```
MyWebApiProject
│
├── Properties
│   └── launchSettings.json
├── appsettings.json
└── Program.cs
```



### 2.简单修改下测试

`Program.cs`

```c#
using Melphi.WebApi;
using Org.BouncyCastle.Asn1.Ocsp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/api/auth", () =>
{
    return Results.Ok(new { message = "hello, here is wemove." });
});

app.Run();

```

`launchSettings.json`

```json
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:50446",
      "sslPort": 0
    }
  },
  "profiles": {
    "Melphi.WebApi": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "api/auth",
      "applicationUrl": "http://localhost:5279",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "api/auth",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}

```

上面简单将默认的`GET`接口修改成我们的接口路径`/api/auth`并让用户如果可以访问把返回一个`hello, here is wemove`

启动调试，默认会打开浏览器，并访问这个接口，在`launchSettings.json`中的配置。

我们也可以自己打开ApiPost之类的测试工具进行测试。



### 3.添加引用

- MeiOrm 



### 4.添加数据库访问类

`WemoveDbService.cs`

```c#
using MeiOrm;

namespace Melphi.WebApi
{
    public class WemoveDbService : BaseSqlService
    {
        public WemoveDbService() :
            base("wemove",
                new MeiOrm.SqlConnectionConfig()
                {
                    Password = "meicreated@2024",
                    Database = "wemove"
                }
            )
        {

        }
    }

    public class user_info
    {
        public int Id { get; set; }
        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public DateTime CreateDate { get; set; }
    }
}

```

`user_info`是数据库表的实体类，为了后续Dapper数据查询反馈类型的定义。

创建完成后，文件夹目录

```
MyWebApiProject
│
├── Properties
│   └── launchSettings.json
├── appsettings.json
├── Program.cs
└── WemoveDbService.cs
```



### 5.邮箱验证

`/api/auth/sign-in-email`

```c#
using Melphi.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("/api/auth", () =>
{
    return Results.Ok(new { message = "hello, here is wemove." });
});

var dbService = new WemoveDbService();

app.MapPost("/api/auth/sign-in-email", (WemoveRequest request) =>
{
    // SQL 查询用户
    var sql = "SELECT * FROM user_info WHERE Email = @Email";
    var user = dbService.QueryFirstOrDefault<user_info>(sql, new { Email = request.Email });
    if (user == null)
    {
        return Results.NotFound(new
        {
            status = "error",
            message = "no email found on the server."
        });
    }
    return Results.Ok(new { message = "sign email successful", user = user });
});

app.Run();


public class WemoveRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}
```

`WemoveRequest`是Post提交的表单，测试时，POST的`row` 格式 `json`参考如下

```json
{
  "Email": "mel@wem.com",
  "Password": ""
}
```

查询成功反馈`json`参考如下

```json
{
	"message": "sign email successful",
	"user": {
		"username": "melphi",
		"email": "mel@wem.com"
	}
}
```

失败反馈

```json
{
	"status": "error",
	"message": "no email found on the server."
}
```



### 6.密码验证

`/api/auth/sign-in-password`

```C#

app.MapPost("/api/auth/sign-in-password", (WemoveRequest request) =>
{
    // SQL 查询用户
    var sql = "SELECT * FROM user_info WHERE Email = @Email";
    var user = dbService.QueryFirstOrDefault<user_info>(sql, new { Email = request.Email });
    if (user == null)
    {
        return Results.NotFound(new
        {
            status = "error",
            message = "no email found on the server."
        });
    }

    if(user.Password== request.Password)
    {
        return Results.Ok(new { message = "sign in password successful", user = new { username = user.UserName, email = user.Email } });
    }
    else
    {
        return Results.NotFound(new
        {
            status = "error",
            message = "password is incorrect."
        });
    }
});
```

测试时，POST的`row` 格式 `json`参考如下

```json
{
  "Email": "mel@wem.com",
  "Password": "deamon"
}
```

查询成功反馈`json`参考如下

```json
{
	"message": "sign in password successful",
	"user": {
		"username": "melphi",
		"email": "mel@wem.com"
	}
}
```

失败反馈，email填写错误

```json
{
	"status": "error",
	"message": "no email found on the server."
}
```

失败反馈，密码不匹配

```json
{
	"status": "error",
	"message": "password is incorrect."
}
```



### 7.用户注册

`/api/auth/sign-in-password`

```C#
app.MapPost("/api/auth/sign-up", (SignUpWemoveRequest request) =>
{
    if (request.Password == null || request.Password.Length < 6 || request.Email == null)
    {
        return Results.BadRequest(new
        {
            status = "error",
            message = "password is null or email is null or password length < 6"
        });
    }

    var user = new user_info() { UserName = request.UserName, Email = request.Email, Password = request.Password, CreateDate = DateTime.Now };
    string sql = @"INSERT INTO user_info (UserName, Email, Password, CreateDate) VALUES (@UserName, @Email, @Password, @CreateDate);";

    var result = dbService.Insert(sql, user);
    if (result.Success)
    {
        return Results.Ok(new { message = result.Message });
    }
    else
    {
        return Results.Problem(result.Message);
    }
});
```

测试时，POST的`row` 格式 `json`参考如下

```json
{
  "Email": "mel@wem.com",
  "UserName":"mei",
  "Password": "deamon"
}
```

插入成功反馈`json`参考如下

```json
{
	"message": "insert successful"
}
```

失败反馈，填写错误

```json
{
	"status": "error",
	"message": "password is null or email is null or password length < 6"
}
```

失败反馈，数据库掉线等

```json
{
	"type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
	"title": "An error occurred while processing your request.",
	"status": 500,
	"detail": "mysql exception: Unable to connect to any of the specified MySQL hosts"
}
```





##  WPF调用WebApi

在 WPF 中，使用 `HttpClient` 来调用Web API

```csharp
 /// <summary>
 /// Email verification
 /// </summary>
 /// <returns></returns>
 private async Task Sign()
 {
     // 输入邮箱格式校验
     if (!Email.IsEmail())
     {
         ErrorMessage = "错误！邮箱格式不正确，请重试！";
         HasError = true;
         WipeErrorAffterMS();
         return;
     }

     var http = "http://localhost:5000";
     var api = "/api/auth/sign-in-email";
     var requestUrl = http + api;

     using (var client = new HttpClient())
     {
         var postData = new
         {
             Email = email,
             Password = ""
         };
         var json = JsonSerializer.Serialize(postData);
         var content = new StringContent(json, Encoding.UTF8, "application/json");
         HttpResponseMessage response = null;
         try
         {
             response = await client.PostAsync(requestUrl, content);
         }
         catch (Exception ex)
         {
             ErrorMessage = ex.Message;
             HasError = true;
             WipeErrorAffterMS();
             return;
         }

         if (response.IsSuccessStatusCode)
         {
             App.Current.Dispatcher.Invoke(() => ServiceProvider.Get<SignViewModel>().CurrentViewType = SignViewType.SignInPass);
         }
         else
         {
             var contentString = await response.Content.ReadAsStringAsync();
             ApiResponse apiResponse = null;
             try
             {
                 apiResponse = JsonSerializer.Deserialize<ApiResponse>(contentString);
                 ErrorMessage = apiResponse == null ? "response content json deserialize error" : apiResponse.message;
             }
             catch (Exception ex)
             {
                 ErrorMessage = ex.Message;
             }

             HasError = true;
             WipeErrorAffterMS();
             return;
         }
     }
 }

 public class ApiResponse
 {
     public string message { get; set; }
 }
```



