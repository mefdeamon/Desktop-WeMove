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
    return Results.Ok(new { message = "sign in email successful", user = new { username = user.UserName, email = user.Email } });
});


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

    if (user.Password == request.Password)
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


app.Run();


public class WemoveRequest
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class SignUpWemoveRequest : WemoveRequest
{
    public string? UserName { get; set; }
}