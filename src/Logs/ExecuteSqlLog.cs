namespace Xunet.Core.Logs;

/// <summary>
/// SQL日志
/// </summary>
public class ExecuteSqlLog
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// SQL命令
    /// </summary>
    public string? CommandText { get; set; }

    /// <summary>
    /// 耗时(单位：秒)
    /// </summary>
    public double? ElapsedTime { get; set; }

    /// <summary>
    /// 是否执行失败(0：成功；1：失败)
    /// </summary>
    public int? IsFail { get; set; }

    /// <summary>
    /// 执行SQL错误消息
    /// </summary>
    public string? Massage { get; set; }

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
