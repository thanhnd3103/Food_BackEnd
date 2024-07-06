using System.Net;
using AutoMapper;
using BLL.Services.Interfaces;
using Common.Constants;
using Common.RequestObjects.Transaction;
using Common.ResponseObjects;
using Common.ResponseObjects.Transaction;
using Common.Status;
using DAL.Repositories;

namespace BLL.Services.Implementation;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TransactionService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public ResponseObject UpdateTransaction(UpdateTransactionRequest updateTransactionRequest, int orderId)
    {
        var transaction = _unitOfWork.TransactionRepository.Get(x => x.OrderID == orderId).FirstOrDefault();
        if (transaction is null)
            return new ResponseObject()
            {
                Result = null,
                Message = Messages.TransactionMessage.GET_TRANSACTION_BY_ORDER_ID,
                StatusCode = HttpStatusCode.BadRequest
            };
        try
        {
            transaction.BankCode = updateTransactionRequest.bankCode;
            transaction.Status = TransactionHistoryStatus.PAID;
            _ = _unitOfWork.Save();
            var response = _mapper.Map<TransactionResponse>(transaction);
            return new ResponseObject()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Result = response,
                Message = Messages.TransactionMessage.UPDATE_TRANSACTION_SUCCESS
            };
        }
        catch (Exception e)
        {
            transaction.BankCode = updateTransactionRequest.bankCode;
            transaction.Status = TransactionHistoryStatus.ERROR;
            _ = _unitOfWork.Save();
            var response = _mapper.Map<TransactionResponse>(transaction);
            return new ResponseObject()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Result = response,
                Message = Messages.TransactionMessage.UPDATE_TRANSACTION_FAIL
            };
        }
    }
}