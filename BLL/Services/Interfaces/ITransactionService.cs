using Common.RequestObjects.Transaction;
using Common.ResponseObjects;

namespace BLL.Services.Interfaces;

public interface ITransactionService
{
    ResponseObject UpdateTransaction(UpdateTransactionRequest updateTransactionRequest, int orderId);
}