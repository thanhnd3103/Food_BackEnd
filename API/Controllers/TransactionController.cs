using BLL.Services.Interfaces;
using Common.RequestObjects.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/api")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPut("/orders/{orderId}/transaction")]
    public ActionResult<object> UpdateTransaction([FromBody] UpdateTransactionRequest updateTransactionRequest, int orderId)
    {
        var response = _transactionService.UpdateTransaction(updateTransactionRequest, orderId);
        return response;
    }
}