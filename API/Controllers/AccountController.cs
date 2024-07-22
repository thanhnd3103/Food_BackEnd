using BLL.Services.Interfaces;
using Common.RequestObjects.Account;
using Common.ResponseObjects.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("accounts/{accountId}")]
    [Authorize]
    public ActionResult<object> GetAccountById(int accountId)
    {
        var response = _accountService.GetAccountById(accountId);
        return response;
    }
    [HttpGet("accounts/current")]
    [Authorize]
    public ActionResult<object> GetCurrentAccountInfo()
    {
        var accountId = HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;
        return _accountService.GetAccountById(Int32.Parse(accountId!));
    }

    [HttpPut("accounts")]
    [Authorize]
    [ProducesResponseType(typeof(AccountResponse), 200)]
    public ActionResult<object> UpdateCurrentAccountInfo(UpdateAccountRequest request)
    {
        var accountId = HttpContext.User.FindFirst(ClaimTypes.Sid)?.Value;
        return _accountService.UpdateAccount(request, Int32.Parse(accountId!));
    }
}