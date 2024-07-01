using Common.ResponseObjects;

namespace BLL.Services.Interfaces;

public interface ITagService
{
    ResponseObject GetAllTag();
    ResponseObject GetAllTagByName(string tagName);
}