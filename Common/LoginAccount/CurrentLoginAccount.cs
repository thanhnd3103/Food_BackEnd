
using System.Security.Claims;
using BLL.Utilities.LoginAccount.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Utilities.LoginAccount;

public class CurrentLoginAccount : ControllerBase, ICurrentLoginAccount
{
    public string? getAccount()
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;
        if (userId is null)
            return null;
        return userId;
    }
}