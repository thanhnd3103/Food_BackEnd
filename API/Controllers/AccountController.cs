using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("/{accountId}")]
    [Authorize]
    public ActionResult<object> GetAccountById(int accountId)
    {
        var response = _accountService.GetAccountById(accountId);
        return response;
    }
    [HttpGet("current")]
    [Authorize]
    public ActionResult<object> GetCurrentAccountInfo()
    {
        var accountId = HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;
        return _accountService.GetAccountById(Int32.Parse(accountId!));
    }
}