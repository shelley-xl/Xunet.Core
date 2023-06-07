namespace Xunet.Core.Helpers;

/// <summary>
/// 阿里云OSS助手
/// </summary>
public class AliyunOssHelper
{
    static IConfiguration Configuration
        => HttpContextHelper.Current!.RequestServices.GetRequiredService<IConfiguration>();

    static string AccessKeyId
        => Configuration["AliyunConfig:OSS:AccessKeyId"]!;

    static string AccessKeySecret
        => Configuration["AliyunConfig:OSS:AccessKeySecret"]!;

    /// <summary>
    /// 上传文件
    /// </summary>
    /// <param name="url"></param>
    /// <param name="area"></param>
    /// <param name="bucket"></param>
    /// <param name="key"></param>
    /// <param name="content"></param>
    public static void PutObject(ref string url, string area = "shanghai", string bucket = "51xulai", string key = "unkown", Stream? content = null)
    {
        var endpoint = $"oss-cn-{area}.aliyuncs.com";
        new OssClient(endpoint, AccessKeyId, AccessKeySecret).PutObject(bucket, key, content);
        url = $"https://{bucket}.{endpoint}/{key}";
    }

    /// <summary>
    /// 列举存储空间
    /// </summary>
    /// <param name="area"></param>
    /// <returns></returns>
    public static List<object> ListBuckets(string area = "shanghai")
    {
        var list = new List<object>();
        var endpoint = $"oss-cn-{area}.aliyuncs.com";
        var buckets = new OssClient(endpoint, AccessKeyId, AccessKeySecret).ListBuckets().OrderBy(x => x.CreationDate);
        foreach (var bucket in buckets)
        {
            list.Add(new { value = bucket.Name, text = bucket.Name });
        }
        return list;
    }

    /// <summary>
    /// 列举文件
    /// </summary>
    /// <param name="nextMarker"></param>
    /// <param name="area"></param>
    /// <param name="bucket"></param>
    /// <param name="pageSize"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public static List<object> ListObjects(ref string nextMarker, string area = "shanghai", string bucket = "51xulai", int pageSize = 10, string prefix = "")
    {
        var list = new List<object>();
        var endpoint = $"oss-cn-{area}.aliyuncs.com";
        var result = new OssClient(endpoint, AccessKeyId, AccessKeySecret).ListObjects(new ListObjectsRequest(bucket) { Marker = nextMarker, MaxKeys = pageSize, Prefix = prefix });
        foreach (var summary in result.ObjectSummaries)
        {
            list.Add(summary);
        }
        nextMarker = result.NextMarker;
        return list;
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    /// <param name="area"></param>
    /// <param name="bucket"></param>
    /// <param name="key"></param>
    public static void DeleteObject(string area = "shanghai", string bucket = "51xulai", string key = "")
    {
        var endpoint = $"oss-cn-{area}.aliyuncs.com";
        new OssClient(endpoint, AccessKeyId, AccessKeySecret).DeleteObject(bucket, key);
    }
}