---
typora-root-url: ./
---



# <img src="/docs/resource/readme.image/log48.png" alt="email" />Desktop-WeMove

## <img src="/docs/resource/readme.image/log48.png" alt="summary"/> Summary（概要）

- Wemove is a practical project of WPF technology sharing.
- Wemove是一个练习WPF技术的共享项目。



## <img src="/docs/resource/readme.image/log48.png" alt="email"/> Screenshots（截图）

- sign in email page（email verification )登录（邮箱验证）

<img src="/docs/resource/readme.image/signin.png" alt="email" style="zoom:100%;" align='left'/>



- sign in password page（password verification )登录（密码验证）

<img src="/docs/resource/readme.image/signinpwd.png" alt="password" align='left' style="zoom:100%;" />



- sign up page（注册）

<img src="/docs/resource/readme.image/signup.png" alt="password" align='left' style="zoom:100%;" />



- main page（主页）

<img src="/docs/resource/readme.image/main.png" alt="main" align='left' style="zoom:80%;" />



## 数据库

数据库表内容

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

C#链接字符串（本地数据库）

```c#
 string ConnectionString = @"server = 127.0.0.1; userid = root; password = deamon; database = wemove; persistsecurityinfo = True;";
```







