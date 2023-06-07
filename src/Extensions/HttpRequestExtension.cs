namespace Xunet.Core.Extensions;

/// <summary>
/// HttpRequest扩展
/// </summary>
public static class HttpRequestExtension
{
    /// <summary>
    /// 获取请求的完整URL地址
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static string GetAbsoluteUri(this HttpRequest request)
    {
        return new StringBuilder()
            .Append(request.Scheme)
            .Append("://")
            .Append(request.Host)
            .Append(request.PathBase)
            .Append(request.Path)
            .Append(request.QueryString)
            .ToString();
    }
}
