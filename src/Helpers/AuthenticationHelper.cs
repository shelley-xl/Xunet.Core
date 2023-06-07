namespace Xunet.Core.Helpers;

/// <summary>
/// 认证助手
/// </summary>
public class AuthenticationHelper
{
    /// <summary>
    /// 当前上下文对象
    /// </summary>
    public static HttpContext? Current => HttpContextHelper.Current;

    /// <summary>
    /// 账户Id
    /// </summary>
    public static string? AccountId
    {
        get
        {
            if (Current != null)
            {
                var claim = Current.User.Claims.SingleOrDefault(x => x.Type == "Id");
                if (claim != null)
                {
                    return claim.Value;
                }
            }
            return null;
        }
    }
}