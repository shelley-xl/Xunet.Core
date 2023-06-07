namespace Xunet.Core.Logs;

/// <summary>
/// 异常日志
/// </summary>
public class ExceptionLog
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 堆栈信息
    /// </summary>
    public string? StackTrace { get; set; }

    /// <summary>
    /// 内部信息
    /// </summary>
    public string? InnerException { get; set; }

    /// <summary>
    /// 异常类型
    /// </summary>
    public string? ExceptionType { get; set; }

    /// <summary>
    /// 服务器
    /// </summary>
    public string? ServerHost { get; set; }

    /// <summary>
    /// 客户端
    /// </summary>
    public string? ClientHost { get; set; }

    /// <summary>
    /// 运行环境
    /// </summary>
    public string? Runtime { get; set; }

    /// <summary>
    /// 请求Url
    /// </summary>
    public string? RequestUrl { get; set; }

    /// <summary>
    /// 请求数据
    /// </summary>
    public string? RequestBody { get; set; }

    /// <summary>
    /// 浏览器代理
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// 请求方式
    /// </summary>
    public string? Method { get; set; }

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
