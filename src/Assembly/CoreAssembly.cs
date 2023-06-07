global using System.Reflection;
global using Aliyun.OSS;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Xunet.Core;
global using Xunet.Core.Dtos;
global using Xunet.Core.Extensions;
global using FluentValidation;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using NLog;
global using NLog.Config;
global using System.Collections.Concurrent;
global using System.Text;
global using System.ComponentModel;
global using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
global using Microsoft.OpenApi.Models;
global using FluentValidation.AspNetCore;
global using Microsoft.Extensions.Hosting;
global using Xunet.Helpers;
global using Xunet.Core.Helpers;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.Extensions.Configuration;
global using System.Linq.Expressions;
global using Autofac;
global using Autofac.Extensions.DependencyInjection;
global using Newtonsoft.Json.Converters;
global using Newtonsoft.Json.Serialization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using System.Security.Claims;
global using Microsoft.IdentityModel.Tokens;
global using System.IdentityModel.Tokens.Jwt;
global using Microsoft.AspNetCore.Authorization;
global using Xunet.Core.Authorization;
global using Xunet.Core.Authentication;
global using Newtonsoft.Json;
global using System.Web;
global using System.Net;
global using Newtonsoft.Json.Linq;
global using System.Diagnostics;
global using Microsoft.Net.Http.Headers;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Xunet.Core.Logs;
global using FluentScheduler;

namespace Xunet.Core;

/// <summary>
/// Core程序集
/// </summary>
public class CoreAssembly
{
    /// <summary>
    /// 程序集对象
    /// </summary>
    public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
}