namespace Microsoft.AspNetCore.Mvc;

/// <summary>
/// 控制器基类
/// </summary>
[ApiController]
public class BaseController : ControllerBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [NonAction]
    protected virtual IActionResult XunetResult()
    {
        return new OkResult();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [NonAction]
    protected virtual IActionResult XunetResult(object? value)
    {
        return new OkObjectResult(value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    [NonAction]
    protected virtual IActionResult XunetResult<TValue>(TValue value) where TValue : OperateResultDto, new()
    {
        value.Timestamp = DateTime.Now.ToTimeStamp();
        value.Path = Request.Path.Value;
        return new OkObjectResult(value);
    }
}