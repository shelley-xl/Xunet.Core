namespace Xunet.Core.Logs;

/// <summary>
/// 异常日志处理
/// </summary>
public class ExceptionLogHandler : LogHandler<ExceptionLog>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="exception"></param>
    public ExceptionLogHandler(Exception exception) : base(LogMode.ExceptionLog)
    {
        LogInfo = new ExceptionLog
        {
            Message = exception.Message,
            StackTrace = exception.StackTrace,
            ExceptionType = exception.GetType().FullName,
            ServerHost = HttpHelper.GetServerIp(),
            ClientHost = HttpHelper.GetClientIp(),
            Runtime = "Web",
            CreateId = AuthenticationHelper.AccountId,
            CreateTime = DateTime.Now,
            UserAgent = HttpHelper.UserAgent(),
            InnerException = NLogger.GetExceptionFullMessage(exception?.InnerException!)!
        };
        //获取请求信息
        var request = HttpContextHelper.Current?.Request!;
        LogInfo!.RequestUrl = request.GetAbsoluteUri();
        LogInfo!.Method = request.Method;
    }
}
