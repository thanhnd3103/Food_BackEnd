using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("/api")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("/accounts/{accountId}")]
    public ActionResult<object> GetAccountById(int accountId)
    {
        var response = _accountService.GetAccountById(accountId);
        return response;
    }
}