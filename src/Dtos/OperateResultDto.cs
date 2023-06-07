namespace Xunet.Core.Dtos;

/// <summary>
/// 状态码
/// </summary>
public enum XunetStatusCode
{
    /// <summary>
    /// 成功
    /// </summary>
    [Description("成功")]
    Success = 0,

    /// <summary>
    /// 失败
    /// </summary>
    [Description("失败")]
    Failure = 1,

    /// <summary>
    /// 无效的参数
    /// </summary>
    [Description("无效的参数")]
    InvalidParameter = 2,

    /// <summary>
    /// 系统异常
    /// </summary>
    [Description("系统异常")]
    SystemException = 3
}

/// <summary>
/// 操作响应
/// </summary>
public class OperateResultDto
{
    /// <summary>
    /// 状态码
    /// </summary>
    public XunetStatusCode Code { get; set; }

    /// <summary>
    /// 消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    public long? Timestamp { get; set; }

    /// <summary>
    /// 请求路由
    /// </summary>
    public string? Path { get; set; }
}

/// <summary>
/// 查询响应
/// </summary>
/// <typeparam name="T">泛型参数</typeparam>
public class OperateResultDto<T> : OperateResultDto
{
    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }
}

/// <summary>
/// 查询响应
/// </summary>
/// <typeparam name="T">泛型参数</typeparam>
public class PageResultDto<T> : OperateResultDto<T>
{
    /// <summary>
    /// 总记录数
    /// </summary>
    public int? Total { get; set; }
}