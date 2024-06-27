using BLL.Services.Interfaces;
using Common.Constants;
using Common.RequestObjects.AuthController;
using Common.ResponseObjects;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost("register")]
        [ProducesResponseType(200)]
        public ActionResult<ResponseObject> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseObject
                {
                    Message = Messages.General.MODEL_STATE_INVALID,
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = ModelState.Values.SelectMany(v => v.Errors)
                };
            }
            if (request.Password != request.ConfirmPassword)
            {
                return new ResponseObject
                {
                    Message = Messages.AuthController.PASSWORD_NOT_MATCHED,
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = request
                };
            }
            return authService.Register(request);
        }

        [HttpPost("login")]
        public ActionResult<ResponseObject> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new ResponseObject
                {
                    Message = Messages.General.MODEL_STATE_INVALID,
                    StatusCode = HttpStatusCode.BadRequest,
                    Result = ModelState.Values.SelectMany(v => v.Errors)
                };
            }
            return authService.Login(request);
        }


    }
}
