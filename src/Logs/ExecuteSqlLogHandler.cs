namespace Xunet.Core.Logs;

/// <summary>
/// SQL日志处理
/// </summary>
public class ExecuteSqlLogHandler : LogHandler<ExecuteSqlLog>
{
    readonly Stopwatch stopwatch = new();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sql"></param>
    public ExecuteSqlLogHandler(string sql) : base(LogMode.SqlLog)
    {
        LogInfo = new ExecuteSqlLog()
        {
            CommandText = sql,
            CreateId = AuthenticationHelper.AccountId,
            CreateTime = DateTime.Now,
            IsFail = 0
        };
        stopwatch.Start();
    }

    /// <summary>
    /// 写入日志
    /// </summary>
    public override void WriteLog()
    {
        stopwatch.Stop();
        LogInfo!.ElapsedTime = stopwatch.Elapsed.TotalSeconds;
        base.WriteLog();
    }
}
