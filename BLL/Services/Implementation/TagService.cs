using System.Net;
using Amazon.Runtime.Internal.Transform;
using AutoMapper;
using BLL.Services.Interfaces;
using Common.Constants;
using Common.ResponseObjects;
using Common.ResponseObjects.Tag;
using DAL.Repositories;

namespace BLL.Services.Implementation;

public class TagService : ITagService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TagService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public ResponseObject GetAllTag()
    {
        var tags = _unitOfWork.TagRepository.Get().ToList();
        var response = _mapper.Map<List<TagResponse>>(tags);
        return new ResponseObject()
        {
            Result = response,
            Message = Messages.TagMessage.GET_TAGS_SUCCESS,
            StatusCode = HttpStatusCode.OK
        };
    }

    public ResponseObject GetAllTagByName(string tagName)
    {
        var tags = _unitOfWork.TagRepository.Get(x => x.Name.Contains(tagName)).ToList();
        var response = _mapper.Map<List<TagResponse>>(tags);
        return new ResponseObject()
        {
            Result = response,
            Message = Messages.TagMessage.GET_TAGS_SUCCESS,
            StatusCode = HttpStatusCode.OK
        };
    }
}