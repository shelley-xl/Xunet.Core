namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// IApplicationBuilder扩展
/// </summary>
public static class IApplicationBuilderExtension
{
    #region 使用自定义异常中间件
    /// <summary>
    /// 使用自定义异常中间件
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseXunetCustomException(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            string? body = null;
            try
            {
                if (!context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        context.Request.EnableBuffering();
                        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, false, leaveOpen: true);
                        body = await reader.ReadToEndAsync();
                        context.Request.Body.Position = 0;
                    }
                    catch
                    {

                    }
                }
                await next();
            }
            catch (Exception ex)
            {
                //写入日志到数据库
                ExceptionLogHandler exceptionLog = new(ex);
                exceptionLog.LogInfo!.RequestBody = body;
                exceptionLog.WriteLog();
                if (app.ApplicationServices.GetRequiredService<IHostEnvironment>().IsProduction())
                {
                    await context.Response.WriteAsJsonAsync(new OperateResultDto
                    {
                        Code = XunetStatusCode.SystemException,
                        Message = "系统异常，请联系管理员！",
                        Path = context.Request.Path.Value,
                        Timestamp = DateTime.Now.ToTimeStamp()
                    });
                }
                else
                {
                    await context.Response.WriteAsJsonAsync(new OperateResultDto
                    {
                        Code = XunetStatusCode.SystemException,
                        Message = ex.ToString(),
                        Path = context.Request.Path.Value,
                        Timestamp = DateTime.Now.ToTimeStamp()
                    });
                }
            }
        });
        return app;
    }
    #endregion

    #region 使用HttpContextHelper
    /// <summary>
    /// 使用HttpContextHelper
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseXunetHttpContextHelper(this IApplicationBuilder app)
    {
        HttpContextHelper.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
        return app;
    }
    #endregion

    #region 使用Swagger
    /// <summary>
    /// 使用Swagger
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseXunetSwagger(this IApplicationBuilder app)
    {
        if (app.ApplicationServices.GetRequiredService<IHostEnvironment>().IsProduction()) return app;

        app.UseSwagger();
        app.UseSwaggerUI(x =>
        {
            x.RoutePrefix = string.Empty;
            x.DocumentTitle = "API 接口服务";
            x.ConfigObject.AdditionalItems["queryConfigEnabled"] = true;// 解决urls.primaryName无法重定向问题
            x.DefaultModelsExpandDepth(-1); // 不显示Models/Schemas
            x.ShowExtensions();
            x.EnableValidator();
            x.SwaggerEndpoint("/swagger/home/swagger.json", "首页");
        });
        return app;
    }
    #endregion

    #region 使用跨域
    /// <summary>
    /// 使用跨域
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseXunetCors(this IApplicationBuilder app)
    {
        app.UseCors("default");
        return app;
    }
    #endregion

    #region 使用NLog
    /// <summary>
    /// 使用NLog
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseXunetNLog(this IApplicationBuilder app)
    {
        var connectionString = app.ApplicationServices.GetRequiredService<IConfiguration>().GetConnectionString("NLogConnection");
        LogManager.LoadConfiguration($"Configs/nlog.config");
        LogManager.Configuration.Variables["connectionString"] = connectionString;
        // 初始化定时作业
        JobManager.Initialize();
        // 每个月1号0点重新执行一下NLog安装脚本实现自动分表
        JobManager.AddJob(() => LogManager.Configuration.Install(new InstallationContext()), (x) =>
        {
            x.WithName("LogManagerConfigurationInstall");
            x.ToRunNow().AndEvery(1).Months().On(1).At(0, 0);
        });
        return app;
    }
    #endregion
}
