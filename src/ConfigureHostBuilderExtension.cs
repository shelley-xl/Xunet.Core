namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// ConfigureHostBuilder扩展
/// </summary>
public static class ConfigureHostBuilderExtension
{
    #region 配置Autofac
    /// <summary>
    /// 配置Autofac
    /// </summary>
    /// <param name="host"></param>
    /// <param name="dic"></param>
    /// <returns></returns>
    public static ConfigureHostBuilder ConfigureXunetAutofac(this ConfigureHostBuilder host, Dictionary<string, Assembly> dic)
    {
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        host.ConfigureContainer<ContainerBuilder>(builder =>
        {
            foreach (var item in dic)
            {
                builder.RegisterAssemblyTypes(item.Value).Where(t => t.Name.EndsWith(item.Key)).AsImplementedInterfaces();
            }
        });
        return host;
    } 
    #endregion
}
