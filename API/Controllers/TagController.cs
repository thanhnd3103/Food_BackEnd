using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet("/tags")]
    [SwaggerOperation(Summary = "Get all tag available")]
    public ActionResult<object> GetTags()
    {
        var response = _tagService.GetAllTag();
        return response;
    }

    [HttpGet("/tags/search-by-name")]
    [SwaggerOperation(Summary = "Get all tag available by name")]
    public ActionResult<object> GetTagsByName([FromQuery] string tagName)
    {
        var response = _tagService.GetAllTagByName(tagName);
        return response;
    }
}