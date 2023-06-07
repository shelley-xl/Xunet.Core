namespace Xunet.Core.Logs;

/// <summary>
/// 系统用户登录日志实体
/// </summary>
public class LoginLog
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Ip对应地址
    /// </summary>
    public string? IpAddressName { get; set; }

    /// <summary>
    /// 服务器主机名
    /// </summary>
    public string? ServerHost { get; set; }

    /// <summary>
    /// 客户端主机名
    /// </summary>
    public string? ClientHost { get; set; }

    /// <summary>
    /// 浏览器信息
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public string? OsVersion { get; set; }

    /// <summary>
    /// 登录token
    /// </summary>
    public string? LoginToken { get; set; }

    /// <summary>
    /// 登录过期时间
    /// </summary>
    [JsonConverter(typeof(DateTimeFormat))]
    public DateTime? LoginExpiresTime { get; set; }

    /// <summary>
    /// 创建人员
    /// </summary>
    public string? CreateId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [JsonConverter(typeof(DateTimeFormat))]
    public DateTime? CreateTime { get; set; }
}
