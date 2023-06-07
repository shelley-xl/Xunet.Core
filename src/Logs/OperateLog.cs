namespace Xunet.Core.Logs;

/// <summary>
/// 操作日志
/// </summary>
public class OperateLog
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 客户端
    /// </summary>
    public string? ClientHost { get; set; }

    /// <summary>
    /// 服务端IP地址
    /// </summary>
    public string? ServerHost { get; set; }

    /// <summary>
    /// 请求内容大小
    /// </summary>
    public long? ContentLength { get; set; }

    /// <summary>
    /// 请求方式
    /// </summary>
    public string? Method { get; set; }

    /// <summary>
    /// 当前请求Url信息
    /// </summary>
    public string? RequestUrl { get; set; }

    /// <summary>
    /// 上次请求Url信息
    /// </summary>
    public string? UrlReferrer { get; set; }

    /// <summary>
    /// 请求数据
    /// </summary>
    public string? RequestBody { get; set; }

    /// <summary>
    /// 浏览器代理信息
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// 控制器名称
    /// </summary>
    public string? Controller { get; set; }

    /// <summary>
    /// 操作名称
    /// </summary>
    public string? Action { get; set; }

    /// <summary>
    /// Action执行时间(秒)
    /// </summary>
    public double? ActionExecutionTime { get; set; }

    /// <summary>
    /// 页面展示时间(秒)
    /// </summary>
    public double? ResultExecutionTime { get; set; }

    /// <summary>
    /// 响应状态
    /// </summary>
    public int? StatusCode { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 创建人
    /// </summary>
    public string? CreateId { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [JsonConverter(typeof(DateTimeFormat))]
    public DateTime? CreateTime { get; set; }
}
