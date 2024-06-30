using Microsoft.AspNetCore.Http;

namespace BLL.Utilities.AWSHelper
{
    public interface IAWSHelper
    {
        (bool, string) UploadImage(IFormFile imageFile);
    }
}
