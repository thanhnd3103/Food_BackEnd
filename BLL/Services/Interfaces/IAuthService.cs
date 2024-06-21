using Common.RequestObjects.AuthController;
using Common.ResponseObjects;
namespace BLL.Services.Interfaces
{
    public interface IAuthService
    {
        ResponseObject Register(RegisterRequest request);
        ResponseObject Login(LoginRequest request);
    }
}
