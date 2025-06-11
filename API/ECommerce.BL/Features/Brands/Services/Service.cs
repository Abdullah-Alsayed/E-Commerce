using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Brands.Dtos;
using ECommerce.BLL.Features.Brands.Requests;
using ECommerce.BLL.Response;
using ECommerce.BLL.UnitOfWork;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Brands.Services
{
    public class BrandService : IBrandService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IStringLocalizer<BrandService> _localizer;
        private readonly IHostEnvironment _environment;

        private Guid _userId = Guid.Empty;
        private string _userName = Constants.System;
        private string _lang = Constants.Languages.Ar;

        public BrandService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<BrandService> localizer,
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
                cfg.CreateMap<Brand, BrandDto>().ReverseMap();
                cfg.CreateMap<Brand, CreateBrandRequest>().ReverseMap();
                cfg.CreateMap<Brand, UpdateBrandRequest>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion initilize mapper

            #region Get User Data From Token
            _userId = _userContext.UserId.Value;

            _userName = _userContext.UserName.Value;

            _lang = _userContext.Language.Value;
            #endregion
        }

        public async Task<BaseResponse> FindAsync(FindBrandRequest request)
        {
            try
            {
                var brand = await _unitOfWork.ProductModule.Brand.FindAsync(request.ID);
                var result = _mapper.Map<BrandDto>(brand);
                return new BaseResponse<BrandDto>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.View,
                    EntitiesEnum.Brand
                );
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse<BaseGridResponse<List<BrandDto>>>> GetAllAsync(
            GetAllBrandRequest request
        )
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? _lang == Languages.Ar
                        ? nameof(Brand.NameAR)
                        : nameof(Brand.NameEN)
                    : request.SearchBy;

                var result = await _unitOfWork.ProductModule.Brand.GetAllAsync(request);
                var response = _mapper.Map<List<BrandDto>>(result.list);
                return new BaseResponse<BaseGridResponse<List<BrandDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Total = response != null ? result.count : 0,

                    Result = new BaseGridResponse<List<BrandDto>>
                    {
                        Items = response,
                        Total = response != null ? result.count : 0,
                    }
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.GetAll,
                    EntitiesEnum.Brand
                );
                return new BaseResponse<BaseGridResponse<List<BrandDto>>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<BaseResponse> CreateAsync(CreateBrandRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var brand = _mapper.Map<Brand>(request);
                brand = await _unitOfWork.ProductModule.Brand.AddAsync(brand, _userId);
                brand.PhotoPath = await _unitOfWork.ProductModule.Brand.UploadPhotoAsync(
                    request.FormFile,
                    Constants.PhotoFolder.Brands
                );
                var result = _mapper.Map<BrandDto>(brand);

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
                    return new BaseResponse<BrandDto>
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
                await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Create,
                    EntitiesEnum.Brand
                );
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> UpdateAsync(UpdateBrandRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var brand = await _unitOfWork.ProductModule.Brand.FindAsync(request.ID);
                _mapper.Map(request, brand);
                brand.PhotoPath = await _unitOfWork.ProductModule.Brand.UploadPhotoAsync(
                    request.FormFile,
                    Constants.PhotoFolder.Brands,
                    brand.PhotoPath
                );
                _unitOfWork.ProductModule.Brand.Update(brand, _userId);
                var result = _mapper.Map<BrandDto>(brand);

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
                    return new BaseResponse<BrandDto>
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
                await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Update,
                    EntitiesEnum.Brand
                );
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> DeleteAsync(DeleteBrandRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var brand = await _unitOfWork.ProductModule.Brand.FindAsync(request.ID);
                _unitOfWork.ProductModule.Brand.Delete(brand, _userId);
                var result = _mapper.Map<BrandDto>(brand);

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
                    return new BaseResponse<BrandDto>
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
                await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Delete,
                    EntitiesEnum.Brand
                );
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> ToggleActiveAsync(ToggleActiveBrandRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var brand = await _unitOfWork.ProductModule.Brand.FindAsync(request.ID);
                _unitOfWork.ProductModule.Brand.ToggleActive(brand, _userId);
                var result = _mapper.Map<BrandDto>(brand);

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
                    return new BaseResponse<BrandDto>
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
                await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Toggle,
                    EntitiesEnum.Brand
                );
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> GetSearchEntityAsync()
        {
            try
            {
                var result = _unitOfWork.ProductModule.Brand.SearchEntity();
                return new BaseResponse<List<string>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ContentModule.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.Search,
                    EntitiesEnum.Brand
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
            _ = await _unitOfWork.ContentModule.Notification.AddNotificationAsync(
                new Notification
                {
                    CreateBy = _userId,
                    CreateName = _userName,
                    OperationType = action,
                    Entity = EntitiesEnum.Brand
                }
            );

        private async Task LogHistory(OperationTypeEnum action) =>
            await _unitOfWork.ContentModule.History.AddAsync(
                new History
                {
                    UserID = _userId,
                    Action = action,
                    Entity = EntitiesEnum.Brand
                },
                _userId
            );

        #endregion
    }
}
