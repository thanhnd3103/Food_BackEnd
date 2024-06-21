using DAL.Entities;

namespace BLL.Utilities.JWTHelper
{
    public interface IJWTHelper
    {
        string CreateToken(Account account);
    }
}
