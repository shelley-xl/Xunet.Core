namespace Xunet.Core.Authentication;

/// <summary>
/// Jwt生成token
/// </summary>
public class JwtToken
{
    /// <summary>
    /// 生成token
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="jwtConfig"></param>
    /// <returns></returns>
    public static string BuildJwtToken(Claim[] claims, JwtConfig jwtConfig)
    {
        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: jwtConfig.ValidIssuer,
            audience: jwtConfig.ValidAudience,
            claims: claims,
            notBefore: now,
            expires: now.AddSeconds(jwtConfig.Expire),
            signingCredentials: GetSigningCredentials(jwtConfig));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    /// <summary>
    /// 验证Token的参数
    /// </summary>
    /// <param name="jwtConfig"></param>
    /// <returns></returns>
    public static TokenValidationParameters CreateTokenValidationParameters(JwtConfig jwtConfig)
    {
        var keyByteArray = Encoding.ASCII.GetBytes(jwtConfig?.SymmetricSecurityKey!);
        var signingKey = new SymmetricSecurityKey(keyByteArray);
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey,
            ValidateIssuer = true,
            ValidIssuer = jwtConfig?.ValidIssuer,
            ValidateAudience = true,
            ValidAudience = jwtConfig?.ValidAudience,
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true,
        };
    }

    private static SigningCredentials GetSigningCredentials(JwtConfig jwtConfig)
    {
        var keyByteArray = Encoding.ASCII.GetBytes(jwtConfig?.SymmetricSecurityKey!);
        var signingKey = new SymmetricSecurityKey(keyByteArray);
        return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
    }
}
