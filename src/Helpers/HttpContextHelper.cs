namespace Xunet.Core.Helpers;

/// <summary>
/// HttpContextHelper
/// </summary>
public class HttpContextHelper
{
    private static IHttpContextAccessor? _accessor;
    /// <summary>
    /// 当前上下文对象
    /// </summary>
    public static HttpContext? Current => _accessor?.HttpContext;
    internal static void Configure(IHttpContextAccessor accessor) => _accessor = accessor;
}