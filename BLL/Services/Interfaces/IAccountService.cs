using Common.ResponseObjects;

namespace BLL.Services.Interfaces;

public interface IAccountService
{
    ResponseObject GetAccountById(int accountId);
}