using System.Net;
using AutoMapper;
using BLL.Services.Interfaces;
using Common.Constants;
using Common.ResponseObjects;
using Common.ResponseObjects.Account;
using DAL.Repositories;

namespace BLL.Services.Implementation;

public class AccountService : IAccountService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AccountService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public ResponseObject GetAccountById(int accountId)
    {
        var account = _unitOfWork.AccountRepository.GetByID(accountId);
        if (account is null)
        {
            return new ResponseObject()
            {
                Result = null,
                Message = Messages.General.NO_DATA_ERROR,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        var response = _mapper.Map<AccountResponse>(account);
        return new ResponseObject()
        {
            Result = response,
            Message = Messages.AccountMessage.GET_ACCOUNT_BY_ID_SUCCESS,
            StatusCode = HttpStatusCode.OK
        };
    }
}