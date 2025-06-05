using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Sliders.Dtos;
using ECommerce.BLL.Features.Sliders.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Sliders.Services
{
    public class SliderService : ISliderService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IStringLocalizer<SliderService> _localizer;
        private readonly IHostEnvironment _environment;

        private Guid _userId = Guid.Empty;
        private string _userName = Constants.System;
        private string _lang = Languages.Ar;

        public SliderService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<SliderService> localizer,
            IUserContext userContext,
            IHostEnvironment environment
        )
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _userContext = userContext;
            _environment = environment;

            #region initilize mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AllowNullCollections = true;
                cfg.CreateMap<Slider, SliderDto>().ReverseMap();
                cfg.CreateMap<Slider, CreateSliderRequest>().ReverseMap();
                cfg.CreateMap<Slider, UpdateSliderRequest>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion initilize mapper

            #region Get User Data From Token
            _userId = _userContext.UserId.Value;

            _userName = _userContext.UserName.Value;

            _lang = _userContext.Language.Value;
            #endregion
        }

        public async Task<BaseResponse> FindAsync(FindSliderRequest request)
        {
            try
            {
                var Slider = await _unitOfWork.Slider.FindAsync(request.ID);
                var result = _mapper.Map<SliderDto>(Slider);
                return new BaseResponse<SliderDto>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.View,
                    EntitiesEnum.Slider
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse<BaseGridResponse<List<SliderDto>>>> GetAllAsync(
            GetAllSliderRequest request
        )
        {
            try
            {
                var result = await _unitOfWork.Slider.GetAllAsync(request);
                var response = _mapper.Map<List<SliderDto>>(result.list);
                return new BaseResponse<BaseGridResponse<List<SliderDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Total = result.count,
                    Result = new BaseGridResponse<List<SliderDto>>
                    {
                        Items = response,
                        Total = result.count,
                    }
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.GetAll,
                    EntitiesEnum.Slider
                );
                return new BaseResponse<BaseGridResponse<List<SliderDto>>>
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> CreateAsync(CreateSliderRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var Slider = _mapper.Map<Slider>(request);
                Slider = await _unitOfWork.Slider.AddAsync(Slider, _userId);
                Slider.PhotoPath = await _unitOfWork.Slider.UploadPhotoAsync(
                    request.FormFile,
                    PhotoFolder.Slider
                );
                var result = _mapper.Map<SliderDto>(Slider);

                //#region Send Notification
                //await SendNotification(OperationTypeEnum.Create);
                //modifyRows++;
                //#endregion

                //#region Log
                //await LogHistory(OperationTypeEnum.Create);
                //modifyRows++;
                //#endregion

                modifyRows++;
                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<SliderDto>
                    {
                        IsSuccess = true,
                        Message = _localizer[MessageKeys.Success].ToString(),
                        Result = result
                    };
                }
                else
                {
                    await transaction.RollbackAsync();
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = _localizer[MessageKeys.Fail].ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Create,
                    EntitiesEnum.Slider
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UpdateAsync(UpdateSliderRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var slider = await _unitOfWork.Slider.FindAsync(request.ID);
                _mapper.Map(request, slider);
                slider.PhotoPath = await _unitOfWork.Slider.UploadPhotoAsync(
                    request.FormFile,
                    PhotoFolder.Slider,
                    slider.PhotoPath
                );
                _unitOfWork.Slider.Update(slider, _userId);
                var result = _mapper.Map<SliderDto>(slider);

                //#region Send Notification
                //await SendNotification(OperationTypeEnum.Update);
                //modifyRows++;
                //#endregion

                //#region Log
                //await LogHistory(OperationTypeEnum.Update);
                //modifyRows++;
                //#endregion

                modifyRows++;
                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<SliderDto>
                    {
                        IsSuccess = true,
                        Message = _localizer[MessageKeys.Success].ToString(),
                        Result = result
                    };
                }
                else
                {
                    await transaction.RollbackAsync();
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = _localizer[MessageKeys.Fail].ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Update,
                    EntitiesEnum.Slider
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> DeleteAsync(DeleteSliderRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var slider = await _unitOfWork.Slider.FindAsync(request.ID);
                _unitOfWork.Slider.Delete(slider, _userId);
                var result = _mapper.Map<SliderDto>(slider);

                //#region Send Notification
                //await SendNotification(OperationTypeEnum.Delete);
                //modifyRows++;
                //#endregion

                //#region Log
                //await LogHistory(OperationTypeEnum.Delete);
                //modifyRows++;
                //#endregion

                modifyRows++;
                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<SliderDto>
                    {
                        IsSuccess = true,
                        Message = _localizer[MessageKeys.Success].ToString(),
                        Result = result
                    };
                }
                else
                {
                    await transaction.RollbackAsync();
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = _localizer[MessageKeys.Fail].ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Delete,
                    EntitiesEnum.Slider
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> ToggleActiveAsync(ToggleActiveSliderRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var Slider = await _unitOfWork.Slider.FindAsync(request.ID);
                _unitOfWork.Slider.ToggleActive(Slider, _userId);
                var result = _mapper.Map<SliderDto>(Slider);

                //#region Send Notification
                //await SendNotification(OperationTypeEnum.Toggle);
                //modifyRows++;
                //#endregion

                //#region Log
                //await LogHistory(OperationTypeEnum.Toggle);
                //modifyRows++;
                //#endregion

                modifyRows++;
                if (await _unitOfWork.IsDone(modifyRows))
                {
                    await transaction.CommitAsync();
                    return new BaseResponse<SliderDto>
                    {
                        IsSuccess = true,
                        Message = _localizer[MessageKeys.Success].ToString(),
                        Result = result
                    };
                }
                else
                {
                    await transaction.RollbackAsync();
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = _localizer[MessageKeys.Fail].ToString(),
                    };
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Toggle,
                    EntitiesEnum.Slider
                );
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> GetSearchEntityAsync()
        {
            try
            {
                var result = _unitOfWork.Slider.SearchEntity();
                return new BaseResponse<List<string>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Search,
                    EntitiesEnum.Slider
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        #region helpers
        private async Task SendNotification(OperationTypeEnum action) =>
            _ = await _unitOfWork.Notification.AddNotificationAsync(
                new Notification
                {
                    CreateBy = _userId,
                    CreateName = _userName,
                    OperationType = action,
                    Entity = EntitiesEnum.Slider
                }
            );

        private async Task LogHistory(OperationTypeEnum action) =>
            await _unitOfWork.History.AddAsync(
                new History
                {
                    UserID = _userId,
                    Action = action,
                    Entity = EntitiesEnum.Slider
                },
                _userId
            );

        #endregion
    }
}
