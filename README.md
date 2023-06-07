# 常用中间件

Xunet.Core 是 .NET Core 的通用中间件库，包含一些常用中间件，用于简化 WebApi 开发。

支持 .NET 6.0、.NET 7.0

[![Xunet.Core](https://img.shields.io/nuget/v/Xunet.Core.svg?style=flat-square)](https://www.nuget.org/packages/Xunet.Core)
[![Xunet.Core](https://img.shields.io/nuget/dt/Xunet.Core.svg?style=flat-square)](https://www.nuget.org/stats/packages/Xunet.Core?groupby=Version)

- 全局异常处理中间件、异常日志

- NLog日志中间件、操作日志、登录日志，自动分表

- 缓存中间件、Redis缓存

- JWT认证和授权

- ORM中间件、SqlSugar、SQL执行日志

- 对象映射中间件、AutoMapper

- Swagger中间件

- 依赖注入Autofac

- FluentValidation验证中间件

- 跨域中间件

```c#
// Program.cs

var builder = WebApplication.CreateBuilder(args);

// 配置Autofac容器 => 批量注入
builder.Host.ConfigureXunetAutofac(new Dictionary<string, Assembly>
{
    { "Service" , ServiceAssembly.Assembly },
    { "Repository" , RepositoryAssembly.Assembly }
});

// 添加Session
builder.Services.AddSession(options => 
{
    options.IdleTimeout = TimeSpan.FromMinutes(20);//设置session过期时间
    options.Cookie.IsEssential = true;
});

// 添加Controllers
builder.Services.AddXunetControllers();
// 添加HttpContextHelper
builder.Services.AddXunetHttpContextHelper();
// 添加FluentValidation
builder.Services.AddXunetFluentValidation(ServiceAssembly.Assembly);
// 添加AutoMapper
builder.Services.AddXunetAutoMapper(ServiceAssembly.Assembly);
// 添加Swagger
builder.Services.AddXuetSwagger("Xunet.WebApi", "Xunet.Entity", "Xunet.Core");
// 添加缓存
builder.Services.AddXunetCache();
// 添加SqlSugar
builder.Services.AddXunetSqlSugar();
// 添加跨域
builder.Services.AddXunetCors();
// 添加认证
builder.Services.AddXunetAuthentication();
// 添加授权
builder.Services.AddXunetAuthorization<PermissionHandler>();

var app = builder.Build();

// 使用Session
app.UseSession();
// 使用异常中间件
app.UseXunetCustomException();
// 使用NLog
app.UseXunetNLog();
// 使用跨域
app.UseXunetCors();
// 使用HttpContextHelper
app.UseXunetHttpContextHelper();
// 使用Swagger
app.UseXunetSwagger();
// 使用SqlSugar => CodeFirst
app.UseXunetSqlSugar(new CodeFirstOptions
{
    EntityAssembly = EntityAssembly.Assembly
    //EntityTypes = new Type[] { } 
});
// 使用路由
app.UseRouting();
// 使用Https
if (app.Environment.IsProduction()) 
{ 
    app.UseHttpsRedirection(); 
}
// 使用认证
app.UseAuthentication();
// 使用授权
app.UseAuthorization();
// 使用控制器
app.MapControllers().RequireAuthorization(AuthorizePolicy.Default);

app.Run();
```
