namespace Xunet.Core.Logs;

/// <summary>
/// 系统用户登录日志处理
/// </summary>
public class LoginLogHandler : LogHandler<LoginLog>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public LoginLogHandler() : base(LogMode.LoginLog)
    {
        LogInfo = new LoginLog
        {
            ServerHost = HttpHelper.GetServerIp(),
            ClientHost = HttpHelper.GetClientIp(),
            UserAgent = HttpHelper.UserAgent(),
            OsVersion = HttpHelper.GetOsVersion(),
            IpAddressName = HttpHelper.GetAddressByApi(),
            CreateTime = DateTime.Now
        };
    }
}
