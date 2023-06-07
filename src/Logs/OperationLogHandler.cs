namespace Xunet.Core.Logs;

/// <summary>
/// 操作日志处理
/// </summary>
public class OperationLogHandler : LogHandler<OperateLog>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="data"></param>
    public OperationLogHandler(HttpRequest request, string data) : base(LogMode.OperateLog)
    {
        LogInfo = new OperateLog
        {
            ServerHost = HttpHelper.GetServerIp(),
            ClientHost = HttpHelper.GetClientIp(),
            ContentLength = request.ContentLength ?? 0,
            Method = request.Method,
            UserAgent = request.Headers[HeaderNames.UserAgent],
            CreateId = AuthenticationHelper.AccountId,
            CreateTime = DateTime.Now,
            RequestBody = data,
            RequestUrl = request.GetAbsoluteUri(),
            UrlReferrer = request.Headers[HeaderNames.Referer]
        };
    }

    /// <summary>
    /// 执行时间
    /// </summary>
    public void ActionExecuted()
    {
        LogInfo!.ActionExecutionTime = Math.Round((DateTime.Now - LogInfo!.CreateTime!).Value.TotalSeconds, 4);
    }

    /// <summary>
    /// 页面展示时间
    /// </summary>
    /// <param name="response"></param>
    public void ResultExecuted(HttpResponse response)
    {
        LogInfo!.StatusCode = response.StatusCode;
        //页面展示时间
        LogInfo!.ResultExecutionTime = Math.Round((DateTime.Now - LogInfo!.CreateTime!).Value.TotalSeconds, 4);
    }
}
