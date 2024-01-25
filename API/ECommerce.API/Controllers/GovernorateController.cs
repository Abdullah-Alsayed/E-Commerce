using AutoMapper;
using ECommerce.BLL.Futures.Governorate.Dtos;
using ECommerce.BLL.Futures.Governorate.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using ECommerce.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize]
    public class GovernorateController : ControllerBase
    {
        private string _userId;
        private string _userName;
        private readonly IUnitOfWork _unitOfWork;

        // private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        IMapper _mapper;

        public GovernorateController(
            IUnitOfWork unitOfWork,
            //  IMapper mapper,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _unitOfWork = unitOfWork;
            //   _mapper = mapper;
            _httpContext = httpContextAccessor;
            _userId = _httpContext.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "ID")
                ?.Value;
            ;
            _userName = _httpContext.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "FullName")
                ?.Value;

            #region initilize mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<Governorate, GovernorateDto>().ReverseMap();
                cfg.CreateMap<Governorate, CreateGovernorateRequest>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion initilize mapper
        }

        [HttpGet]
        public async Task<BaseResponse> FindGovernorate([FromQuery] FindGovernorateRequest request)
        {
            try
            {
                var governorate = await _unitOfWork.Governorate.FindAsync(request.ID);
                var result = _mapper.Map<GovernorateDto>(governorate);
                return new BaseResponse<GovernorateDto>
                {
                    IsSuccess = true,
                    Message = Constants.Messages.Success,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                _ = await _unitOfWork.ErrorLog.AddaAync(
                    new ErrorLog { Source = ex.Source, Message = ex.StackTrace, }
                );
                _ = await _unitOfWork.SaveAsync();
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpGet]
        public async Task<BaseResponse> GetAllGovernorate(
            [FromQuery] GetAllGovernorateRequest request
        )
        {
            try
            {
                var governorates = await _unitOfWork.Governorate.GetAllAsync(request);
                var response = _mapper.Map<List<GovernorateDto>>(governorates);
                return new BaseResponse<BaseGridResponse<List<GovernorateDto>>>
                {
                    IsSuccess = true,
                    Message = Constants.Messages.Success,
                    Result = new BaseGridResponse<List<GovernorateDto>>
                    {
                        Items = response,
                        Total = response != null ? response.Count : 0
                    }
                };
            }
            catch (Exception ex)
            {
                _ = await _unitOfWork.ErrorLog.AddaAync(
                    new ErrorLog { Source = ex.Source, Message = ex.StackTrace, }
                );
                _ = await _unitOfWork.SaveAsync();

                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpGet]
        public async Task<BaseResponse> GetSearchEntity()
        {
            try
            {
                var result = _unitOfWork.Governorate.SearchEntity();
                return new BaseResponse<List<string>>
                {
                    IsSuccess = true,
                    Message = Constants.Messages.Success,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                _ = await _unitOfWork.ErrorLog.AddaAync(
                    new ErrorLog { Source = ex.Source, Message = ex.StackTrace, }
                );
                _ = await _unitOfWork.SaveAsync();

                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPost]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<BaseResponse> CreateGovernorate(CreateGovernorateRequest request)
        {
            try
            {
                var governorate = _mapper.Map<Governorate>(request);
                governorate.CreateBy = _userId;
                governorate = await _unitOfWork.Governorate.AddaAync(governorate);
                var result = _mapper.Map<GovernorateDto>(governorate);
                _ = await _unitOfWork.Notification.AddNotificationAsync(
                    new Notification
                    {
                        CreateBy = _userId,
                        CreateName = _userName,
                        operationTypeEnum = OperationTypeEnum.Create,
                        Icon = Constants.NotificationIcons.Add,
                        Title = "Create Governorate",
                        Subject = "Create Governorate",
                        Message = "Create Governorate",
                    }
                );
                return new BaseResponse<GovernorateDto>
                {
                    IsSuccess = true,
                    Message = Constants.Messages.Success,
                    Result = result
                };
            }
            catch (Exception ex)
            {
                _ = await _unitOfWork.ErrorLog.AddaAync(
                    new ErrorLog { Source = ex.Source, Message = ex.StackTrace, }
                );
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
            finally
            {
                _ = await _unitOfWork.SaveAsync();
            }
        }

        [HttpPut]
        public async Task<BaseResponse> UpdateGovernorate(UpdateGovernorateRequst requst )
        {
            {
                var governorate = _mapper.Map<Governorate>(request);
                governorate.CreateBy = _userId;
                governorate = await _unitOfWork.Governorate.AddaAync(governorate);
                var result = _mapper.Map<GovernorateDto>(governorate);
                _ = await _unitOfWork.Notification.AddNotificationAsync(
                    new Notification
                    {
                        CreateBy = _userId,
                        CreateName = _userName,
                        operationTypeEnum = OperationTypeEnum.Update,
                        Icon = Constants.NotificationIcons.Edit,
                        Title = "Update Governorate",
                        Subject = "Update Governorate",
                        Message = "Update Governorate",
                    }
                );
                return new BaseResponse<GovernorateDto>
                {
                    IsSuccess = true,
                    Message = Constants.Messages.Success,
                    Result = result
                };
            
            catch (Exception ex)
            {
                _ = await _unitOfWork.ErrorLog.AddaAync(
                    new ErrorLog { Source = ex.Source, Message = ex.StackTrace, }
                );
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
            finally
            {
                _ = await _unitOfWork.SaveAsync();
            }

            Governorate Governorate = await _unitOfWork.Governorate.FindAsync(ID);
            if (Governorate == null)
            {
                return BadRequest(Constants.Errors.NotFound);
            }
            else
            {
                _ = _mapper.Map(dto, Governorate);
                Governorate.ModifyAt = DateTime.Now;
                Governorate.ModifyBy = _unitOfWork.User.GetUserID(User);
                _ = await _unitOfWork.SaveAsync();
                return Ok(Governorate);
            }
        }

        [HttpPut]
        //[Authorize(Roles = nameof(Constants.Roles.Admin))]
        public async Task<IActionResult> SetAvtiveGovernorate(int ID)
        {
            Governorate Governorate = await _unitOfWork.Governorate.FindAsync(ID);
            if (Governorate == null)
            {
                return NotFound(Constants.Errors.NotFound);
            }
            else
            {
                Governorate.IsActive = _unitOfWork.Governorate.ToggleAvtive(Governorate.IsActive);
            }

            _ = await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
