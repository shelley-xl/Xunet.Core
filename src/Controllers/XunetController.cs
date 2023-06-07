namespace Microsoft.AspNetCore.Mvc;

/// <summary>
/// 控制器基类（带操作日志）
/// </summary>
[ApiController]
public class XunetController : BaseController, IActionFilter, IResultFilter
{
    private OperationLogHandler? _operationLogHandler = null;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    [NonAction]
    public void OnActionExecuting(ActionExecutingContext context)
    {
        _operationLogHandler = new OperationLogHandler(context.HttpContext.Request, JsonConvert.SerializeObject(context.ActionArguments));
        _operationLogHandler.LogInfo!.Controller = context.ActionDescriptor.RouteValues["controller"]!;
        _operationLogHandler.LogInfo!.Action = context.ActionDescriptor?.RouteValues["action"]!;
        var des = (context.ActionDescriptor as ControllerActionDescriptor)?.MethodInfo.GetCustomAttributes(false).Where(f => f is DescriptionAttribute).FirstOrDefault();
        if (des != null)
        {
            _operationLogHandler.LogInfo!.Description = (des as DescriptionAttribute)?.Description!;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    [NonAction]
    public void OnActionExecuted(ActionExecutedContext context)
    {
        _operationLogHandler?.ActionExecuted();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    [NonAction]
    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result.GetType() == typeof(RedirectResult))
        {
            _operationLogHandler?.ActionExecuted();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    [NonAction]
    public void OnResultExecuted(ResultExecutedContext context)
    {
        _operationLogHandler?.ResultExecuted(context.HttpContext.Response);
        _operationLogHandler?.WriteLog();
    }
}
