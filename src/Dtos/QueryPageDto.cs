namespace Xunet.Core.Dtos;

/// <summary>
/// 分页查询基类
/// </summary>
public class QueryPageDto
{
    /// <summary>
    /// 页码
    /// </summary>
    public int? page { get; set; } = 1;

    /// <summary>
    /// 页大小
    /// </summary>
    public int? size { get; set; } = 20;
}