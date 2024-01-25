using AutoMapper;
using ECommerce.BLL.Futures.Governorate.Dtos;
using ECommerce.BLL.Futures.Governorate.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.DAL.Entity;
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
        private readonly string _userId;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public GovernorateController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContext = httpContextAccessor;
            _userId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
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
        public async Task<BaseResponse> CreateGovernorate(CreateGovernorateRequest dto)
        {
            try
            {
                Governorate Mapping = _mapper.Map<Governorate>(dto);
                Mapping.CreateAt = DateTime.Now;
                Mapping.CreateBy = _unitOfWork.User.GetUserID(User);

                Governorate Governorate = await _unitOfWork.Governorate.AddaAync(Mapping);
                if (Governorate == null)
                {
                    return BadRequest(Constants.Errors.CreateFailed);
                }
                else
                {
                    _ = await _unitOfWork.SaveAsync();
                }

                return Ok(Governorate);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGovernorate(int ID, GovernorateDto dto)
        {
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
