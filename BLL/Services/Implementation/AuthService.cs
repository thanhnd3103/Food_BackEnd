using AutoMapper;
using BLL.Services.Interfaces;
using BLL.Utilities.JWTHelper;
using Common.Constants;
using Common.RequestObjects.AuthController;
using Common.ResponseObjects;
using DAL.Entities;
using DAL.Repositories;


namespace BLL.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTHelper _jwtHelper;
        private readonly IMapper _mapper;
        public AuthService(IUnitOfWork unitOfWork, IJWTHelper jwtHelper, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._jwtHelper = jwtHelper;
            this._mapper = mapper;
        }

        public ResponseObject Register(RegisterRequest request)
        {
            try
            {
                if (_unitOfWork.AccountRepository!.Get(filter: account => account.Email == request.Email)
                                                    .FirstOrDefault() != null)
                {
                    return new ResponseObject
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = Messages.AuthController.EMAIL_ALREADY_EXISTED,
                        Result = null,
                    };
                }

                Account newAccount = _mapper.Map<Account>(request);
                _unitOfWork.AccountRepository.Insert(newAccount);
                _unitOfWork.Save();

                _jwtHelper.CreateToken(newAccount);

                return new ResponseObject
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = Messages.AuthController.REGISTER_SUCCESS,
                    Result = null,
                };

            }
            catch (Exception ex)
            {
                return new ResponseObject
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Message = Messages.General.CRITICAL_UNIDENTIFIED_ERROR,
                    Result = null,
                };
            }
        }

        public ResponseObject Login(LoginRequest request)
        {
            try
            {
                Account loginAccount = _unitOfWork.AccountRepository!.Get(filter: account => account.Email == request.Email).FirstOrDefault();
                if (loginAccount != null && BCrypt.Net.BCrypt.Verify(request.Password, loginAccount.PasswordHash))
                {
                    return new ResponseObject
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Message = Messages.AuthController.LOGIN_SUCCESS,
                        Result = _jwtHelper.CreateToken(loginAccount)
                    };
                }

                return new ResponseObject
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Message = Messages.AuthController.LOGIN_FAILED,
                    Result = null,
                };


            }
            catch (Exception ex)
            {
                return new ResponseObject
                {
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    Message = Messages.General.CRITICAL_UNIDENTIFIED_ERROR,
                    Result = null,
                };
            }
        }
    }
}
