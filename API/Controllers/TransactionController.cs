using BLL.Services.Interfaces;
using Common.RequestObjects.Transaction;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(Summary = "This will update the Transaction's status of the OrderID to PAID, also the bank code ")]
    public ActionResult<object> UpdateTransaction([FromBody] UpdateTransactionRequest updateTransactionRequest, int orderId)
    {
        var response = _transactionService.UpdateTransaction(updateTransactionRequest, orderId);
        return response;
    }
}