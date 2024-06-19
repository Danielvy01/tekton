using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Tekton.Seguridad;
[Route("api/[controller]")]
[ApiController]
[Authorize]
[ExcludeFromCodeCoverage]


/// <summary>
/// SecurityBaseController
/// </summary>
public class SecurityBaseController : ControllerBase
{
    /// <summary>
    /// UserId
    /// </summary>
    protected int? UserId
    {
        get
        {
            var value = User?.FindFirstValue(JwtClaimTypes.Id);
            return string.IsNullOrEmpty(value) ? null : int.Parse(value);
        }
    }

    /// <summary>
    /// UserRoleId
    /// </summary>
    protected string? UserRoleId
    {
        get
        {
            return User?.FindFirstValue("rolId");
        }
    }

    /// <summary>
    /// UserRole
    /// </summary>
    protected string? UserRole
    {
        get
        {
            return User?.FindFirstValue("rol");
        }
    }

    /// <summary>
    /// UserEmail
    /// </summary>
    protected string? UserEmail
    {
        get
        {
            return User?.FindFirstValue(JwtClaimTypes.Email);
        }
    }

    /// <summary>
    /// UserNickName
    /// </summary>
    protected string? UserNickName
    {
        get
        {
            return User?.FindFirstValue(JwtClaimTypes.NickName);
        }
    }

    /// <summary>
    /// UserName
    /// </summary>
    protected string? UserName
    {
        get
        {
            return User?.Identity?.Name;
        }
    }
}
