using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Brands.Dtos;
using ECommerce.BLL.Features.Brands.Dtos;
using ECommerce.BLL.Features.Brands.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Brands.Services
{
    public class BrandService : IBrandService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IStringLocalizer<BrandService> _localizer;
        private readonly IHostEnvironment _environment;

        private string _userId = Constants.System;
        private string _userName = Constants.System;
        private string _lang = Constants.Languages.Ar;

        public BrandService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<BrandService> localizer,
            IHttpContextAccessor httpContextAccessor,
            IHostEnvironment environment
        )
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _httpContext = httpContextAccessor;
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
            _userId = _httpContext
                .HttpContext.User.Claims.FirstOrDefault(x => x.Type == EntitsKeys.ID)
                ?.Value;

            _userName = _httpContext
                .HttpContext.User.Claims.FirstOrDefault(x => x.Type == EntitsKeys.FullName)
                ?.Value;

            _lang =
                _httpContext.HttpContext?.Request.Headers?.AcceptLanguage.ToString()
                ?? Languages.Ar;
            #endregion
        }

        public async Task<BaseResponse> FindAsync(FindBrandRequest request)
        {
            try
            {
                var brand = await _unitOfWork.Brand.FindAsync(request.ID);
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
                _ = await _unitOfWork.ErrorLog.AddaAync(
                    new ErrorLog { Source = ex.Source, Message = ex.StackTrace }
                );
                _ = await _unitOfWork.SaveAsync();
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> GetAllAsync(GetAllBrandRequest request)
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? _lang == Languages.Ar
                        ? nameof(Brand.NameAR)
                        : nameof(Brand.NameEN)
                    : request.SearchBy;

                var brands = await _unitOfWork.Brand.GetAllAsync(request);
                var response = _mapper.Map<List<BrandDto>>(brands);
                return new BaseResponse<BaseGridResponse<List<BrandDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = new BaseGridResponse<List<BrandDto>>
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

        public async Task<BaseResponse> CreateAsync(CreateBrandRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var brand = _mapper.Map<Brand>(request);
                brand.CreateBy = _userId;
                brand = await _unitOfWork.Brand.AddaAync(brand);
                brand.PhotoPath = await _unitOfWork.Brand.UplodPhoto(
                    request.FormFile,
                    _environment,
                    Constants.PhotoFolder.Brands
                );
                var result = _mapper.Map<BrandDto>(brand);
                #region Send Notification
                await SendNotification(OperationTypeEnum.Create);
                modifyRows++;
                #endregion

                #region Log
                await LogHistory(OperationTypeEnum.Create);
                modifyRows++;
                #endregion

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
                await ErrorLog(ex, OperationTypeEnum.Create);
                _ = await _unitOfWork.SaveAsync();
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> UpdateAsync(UpdateBrandRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var brand = await _unitOfWork.Brand.FindAsync(request.ID);
                _mapper.Map(request, brand);
                brand.PhotoPath = await _unitOfWork.Brand.UplodPhoto(
                    request.FormFile,
                    _environment,
                    Constants.PhotoFolder.Brands,
                    brand.PhotoPath
                );
                brand.ModifyBy = _userId;
                brand.ModifyAt = DateTime.UtcNow;
                var result = _mapper.Map<BrandDto>(brand);
                #region Send Notification
                await SendNotification(OperationTypeEnum.Update);
                modifyRows++;
                #endregion

                #region Log
                await LogHistory(OperationTypeEnum.Update);
                modifyRows++;
                #endregion

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
                await ErrorLog(ex, OperationTypeEnum.Update);
                _ = await _unitOfWork.SaveAsync();
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> DeleteAsync(DeleteBrandRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var brand = await _unitOfWork.Brand.FindAsync(request.ID);
                brand.DeletedBy = _userId;
                brand.DeletedAt = DateTime.UtcNow;
                brand.IsDeleted = true;
                var result = _mapper.Map<BrandDto>(brand);
                #region Send Notification
                await SendNotification(OperationTypeEnum.Delete);
                modifyRows++;
                #endregion

                #region Log
                await LogHistory(OperationTypeEnum.Delete);
                modifyRows++;
                #endregion

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
                await ErrorLog(ex, OperationTypeEnum.Delete);
                _ = await _unitOfWork.SaveAsync();
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> ToggleAvtiveAsync(ToggleAvtiveBrandRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var brand = await _unitOfWork.Brand.FindAsync(request.ID);
                brand.ModifyBy = _userId;
                brand.ModifyAt = DateTime.UtcNow;
                brand.IsActive = !brand.IsActive;
                var result = _mapper.Map<BrandDto>(brand);
                #region Send Notification
                await SendNotification(OperationTypeEnum.Toggle);
                modifyRows++;
                #endregion

                #region Log
                await LogHistory(OperationTypeEnum.Toggle);
                modifyRows++;
                #endregion

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
                await ErrorLog(ex, OperationTypeEnum.Toggle);
                _ = await _unitOfWork.SaveAsync();
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> GetSearchEntityAsync()
        {
            try
            {
                var result = _unitOfWork.Brand.SearchEntity();
                return new BaseResponse<List<string>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = result
                };
            }
            catch (Exception ex)
            {
                _ = await _unitOfWork.ErrorLog.AddaAync(
                    new ErrorLog { Source = ex.Source, Message = ex.Message, }
                );
                _ = await _unitOfWork.SaveAsync();
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        #region helpers
        private async Task SendNotification(OperationTypeEnum action)
        {
            _ = await _unitOfWork.Notification.AddNotificationAsync(
                new Notification
                {
                    CreateBy = _userId,
                    CreateName = _userName,
                    OperationType = action,
                    Icon = action.ToString(),
                    Entity = EntitiesEnum.Brand
                }
            );
        }

        private async Task LogHistory(OperationTypeEnum action)
        {
            await _unitOfWork.History.AddaAync(
                new History
                {
                    UserID = _userId,
                    Action = action,
                    Entity = EntitiesEnum.Brand
                }
            );
        }

        private async Task ErrorLog(Exception ex, OperationTypeEnum action)
        {
            _unitOfWork.Context.ChangeTracker.Clear();
            _ = await _unitOfWork.ErrorLog.AddaAync(
                new ErrorLog
                {
                    Source = ex.Source,
                    Message = ex.Message,
                    Operation = action,
                    Entity = EntitiesEnum.Brand
                }
            );
        }

        #endregion
    }
}
