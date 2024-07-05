using Common.RequestObjects.Account;
using Common.ResponseObjects;

namespace BLL.Services.Interfaces;

public interface IAccountService
{
    ResponseObject GetAccountById(int accountId);
    ResponseObject UpdateAccount(UpdateAccountRequest request, int accountId);
}