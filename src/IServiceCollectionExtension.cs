using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// IServiceCollection扩展
/// </summary>
public static class IServiceCollectionExtension
{
    #region 服务是否已注册
    private static readonly ConcurrentDictionary<string, char> keyValuePairs = new();
    /// <summary>
    /// 服务是否已注册
    /// </summary>
    /// <param name="_"></param>
    /// <param name="modelName"></param>
    /// <returns></returns>
    public static bool HasRegistered(this IServiceCollection _, string modelName)
        => !keyValuePairs.TryAdd(modelName.ToLower(), '1');
    #endregion

    #region 添加Controllers
    /// <summary>
    /// 添加Controllers
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetControllers(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetControllers))) return services;

        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                //数据格式按原样输出  --此选项开启默认属性输出 
                //options.SerializerSettings.ContractResolver = new DefaultContractResolver();

                //修改属性名称的序列化方式，首字母小写(属性输出为 小驼峰)
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                //修改时间的序列化方式
                options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

                //将long类型转为string
                //options.SerializerSettings.Converters.Add(new LongToStringConverter());

                //忽略循环引用
                //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

                //忽略空值
                //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

        return services;
    }
    #endregion

    #region 添加HttpContextHelper
    /// <summary>
    /// 添加HttpContextHelper
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetHttpContextHelper(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetHttpContextHelper))) return services;

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
    #endregion

    #region 添加认证
    /// <summary>
    /// 添加认证
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetAuthentication(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetAuthentication))) return services;

        services.Configure<JwtConfig>(services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetSection("JwtConfig"));

        var jwtConfig = services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetSection("JwtConfig").Get<JwtConfig>();
        if (jwtConfig == null) return services;

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = JwtToken.CreateTokenValidationParameters(jwtConfig);
            });

        return services;
    }
    #endregion

    #region 添加授权
    /// <summary>
    /// 添加授权
    /// </summary>
    /// <typeparam name="THandler">AuthorizationHandler</typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetAuthorization<THandler>(this IServiceCollection services) where THandler : AuthorizationHandler<PermissionRequirement>
    {
        if (services.HasRegistered(nameof(AddXunetAuthorization))) return services;

        services.AddScoped<IAuthorizationHandler, THandler>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizePolicy.Default, policy =>
            {
                policy.Requirements.Add(new PermissionRequirement());
            });
        });

        return services;
    }
    #endregion

    #region 添加FluentValidation
    /// <summary>
    /// 添加FluentValidation
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetFluentValidation(this IServiceCollection services, Assembly assembly)
    {
        if (services.HasRegistered(nameof(AddXunetFluentValidation))) return services;

        services.AddFluentValidationAutoValidation(config =>
        {
            // 验证失败，停止验证其他项
            ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
        });
        services.AddValidatorsFromAssembly(assembly);

        services.Configure<ApiBehaviorOptions>(options =>
        {
            // 格式化验证信息
            options.InvalidModelStateResponseFactory = (context) =>
            {
                return new OkObjectResult(new OperateResultDto
                {
                    Code = XunetStatusCode.InvalidParameter,
                    Message = context.ModelState.GetValidationSummary(),
                    Timestamp = DateTime.Now.ToTimeStamp(),
                    Path = context.HttpContext.Request.Path
                });
            };
        });

        return services;
    }

    /// <summary>
    /// 获取模型验证错误消息
    /// </summary>
    /// <param name="modelState"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string? GetValidationSummary(this ModelStateDictionary modelState, string separator = "\r\n")
    {
        if (modelState.IsValid) return null;

        var error = new StringBuilder();

        foreach (var item in modelState)
        {
            var state = item.Value;
            var message = state.Errors.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.ErrorMessage))?.ErrorMessage;
            if (string.IsNullOrWhiteSpace(message))
                message = state.Errors.FirstOrDefault(o => o.Exception != null)?.Exception?.Message;

            if (string.IsNullOrWhiteSpace(message)) continue;

            if (error.Length > 0)
                error.Append(separator);

            error.Append(message);
        }

        return error.ToString();
    }
    #endregion

    #region 添加Swagger
    /// <summary>
    /// 添加Swagger
    /// </summary>
    /// <param name="services"></param>
    /// <param name="xmlFileName"></param>
    /// <returns></returns>
    public static IServiceCollection AddXuetSwagger(this IServiceCollection services, params string[] xmlFileName)
    {
        if (services.HasRegistered(nameof(AddXuetSwagger))) return services;
        if (services.BuildServiceProvider().GetRequiredService<IWebHostEnvironment>().IsProduction()) return services;

        services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("home", new OpenApiInfo
            {
                Title = "API 接口服务",
                Description = "接口说明",
                Version = "v1",
            });
            x.SwaggerDoc("console", new OpenApiInfo
            {
                Title = "控制台",
                Description = "接口说明",
                Version = "v1",
            });
            x.SwaggerDoc("music", new OpenApiInfo
            {
                Title = "酷猫音乐",
                Description = "接口说明",
                Version = "v1",
            });
            x.DocInclusionPredicate((docName, apiDes) =>
            {
                if (!apiDes.TryGetMethodInfo(out MethodInfo method)) return false;
                /*使用ApiExplorerSettingsAttribute里面的GroupName进行特性标识
                 * DeclaringType只能获取controller上的特性
                 * 我们这里是想以action的特性为主
                 * */
                var version = method.DeclaringType!.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                if (docName == "home" && !version.Any()) return true;
                //这里获取action的特性
                var actionVersion = method.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(m => m.GroupName);
                if (actionVersion.Any()) return actionVersion.Any(v => v == docName);
                return version.Any(v => v == docName);
            });
            var scheme = new OpenApiSecurityScheme()
            {
                Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Authorization"
                },
                Scheme = "oauth2",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            };
            x.AddSecurityDefinition("Authorization", scheme);
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                [scheme] = new List<string>()
            });
            foreach (var item in xmlFileName)
            {
                x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{item}.xml"));
            }
        });

        services.AddFluentValidationRulesToSwagger();

        return services;
    }
    #endregion

    #region 添加跨域
    /// <summary>
    /// 添加跨域
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddXunetCors(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddXunetCors))) return services;

        services.AddCors(options =>
        {
            var corsHosts = services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetSection("CorsHosts").Get<string[]>();
            if (corsHosts == null) return;
            if (!corsHosts.Any()) return;
            options.AddPolicy("default", policy =>
            {
                policy
                .WithOrigins(corsHosts)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithExposedHeaders("X-Pagination");
            });
        });

        return services;
    }
    #endregion

    #region 添加OSS歌词文件访问HttpClientFactory
    /// <summary>
    /// 添加OSS歌词文件访问HttpClientFactory
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddOssReferer(this IServiceCollection services)
    {
        if (services.HasRegistered(nameof(AddOssReferer))) return services;

        //添加OSS歌词文件访问HttpClientFactory
        services.AddHttpClient("oss", x =>
        {
            var ossRefererUrl = services.BuildServiceProvider().GetRequiredService<IConfiguration>().GetSection("AliyunConfig:OssReferer").Get<string>();
            if (string.IsNullOrEmpty(ossRefererUrl)) return;
            x.DefaultRequestHeaders.Add("Referer", ossRefererUrl);
            x.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
        });

        return services;
    }
    #endregion
}