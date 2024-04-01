using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Categories.Dtos;
using ECommerce.BLL.Features.Categories.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Categories.Services
{
    public class CategoryService : ICategoryService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IStringLocalizer<CategoryService> _localizer;
        private readonly IHostEnvironment _environment;

        private string _userId = Constants.System;
        private string _userName = Constants.System;
        private string _lang = Constants.Languages.Ar;

        public CategoryService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<CategoryService> localizer,
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
                cfg.CreateMap<Category, CategoryDto>().ReverseMap();
                cfg.CreateMap<Category, CreateCategoryRequest>().ReverseMap();
                cfg.CreateMap<Category, UpdateCategoryRequest>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion initilize mapper

            #region Get User Data From Token
            _userId = _httpContext
                .HttpContext.User.Claims.FirstOrDefault(x => x.Type == EntityKeys.ID)
                ?.Value;

            _userName = _httpContext
                .HttpContext.User.Claims.FirstOrDefault(x => x.Type == EntityKeys.FullName)
                ?.Value;

            _lang =
                _httpContext.HttpContext?.Request.Headers?.AcceptLanguage.ToString()
                ?? Languages.Ar;
            #endregion
        }

        public async Task<BaseResponse> FindAsync(FindCategoryRequest request)
        {
            try
            {
                var Category = await _unitOfWork.Category.FindAsync(request.ID);
                var result = _mapper.Map<CategoryDto>(Category);
                return new BaseResponse<CategoryDto>
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
                    EntitiesEnum.Category
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> GetAllAsync(GetAllCategoryRequest request)
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? _lang == Languages.Ar
                        ? nameof(Category.NameAR)
                        : nameof(Category.NameEN)
                    : request.SearchBy;

                var Categorys = await _unitOfWork.Category.GetAllAsync(request);
                var response = _mapper.Map<List<CategoryDto>>(Categorys);
                return new BaseResponse<BaseGridResponse<List<CategoryDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Result = new BaseGridResponse<List<CategoryDto>>
                    {
                        Items = response,
                        Total = response != null ? response.Count : 0
                    }
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.ErrorLog.ErrorLog(
                    ex,
                    OperationTypeEnum.GetAll,
                    EntitiesEnum.Category
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> CreateAsync(CreateCategoryRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var Category = _mapper.Map<Category>(request);
                Category.CreateBy = _userId;
                Category = await _unitOfWork.Category.AddAsync(Category);
                Category.PhotoPath = await _unitOfWork.Category.UploadPhoto(
                    request.FormFile,
                    _environment,
                    Constants.PhotoFolder.Categorys
                );
                var result = _mapper.Map<CategoryDto>(Category);
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
                    return new BaseResponse<CategoryDto>
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
                    EntitiesEnum.Category
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UpdateAsync(UpdateCategoryRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var Category = await _unitOfWork.Category.FindAsync(request.ID);
                _mapper.Map(request, Category);
                Category.PhotoPath = await _unitOfWork.Category.UploadPhoto(
                    request.FormFile,
                    _environment,
                    Constants.PhotoFolder.Categorys,
                    Category.PhotoPath
                );
                Category.ModifyBy = _userId;
                Category.ModifyAt = DateTime.UtcNow;
                var result = _mapper.Map<CategoryDto>(Category);
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
                    return new BaseResponse<CategoryDto>
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
                    EntitiesEnum.Category
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> DeleteAsync(DeleteCategoryRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var Category = await _unitOfWork.Category.FindAsync(request.ID);
                Category.DeletedBy = _userId;
                Category.DeletedAt = DateTime.UtcNow;
                Category.IsDeleted = true;
                var result = _mapper.Map<CategoryDto>(Category);
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
                    return new BaseResponse<CategoryDto>
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
                    EntitiesEnum.Category
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> ToggleAvtiveAsync(ToggleAvtiveCategoryRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var Category = await _unitOfWork.Category.FindAsync(request.ID);
                Category.ModifyBy = _userId;
                Category.ModifyAt = DateTime.UtcNow;
                Category.IsActive = !Category.IsActive;
                var result = _mapper.Map<CategoryDto>(Category);
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
                    return new BaseResponse<CategoryDto>
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
                    EntitiesEnum.Category
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> GetSearchEntityAsync()
        {
            try
            {
                var result = _unitOfWork.Category.SearchEntity();
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
                    EntitiesEnum.Category
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
                    Entity = EntitiesEnum.Category
                }
            );

        private async Task LogHistory(OperationTypeEnum action) =>
            await _unitOfWork.History.AddAsync(
                new History
                {
                    UserID = _userId,
                    Action = action,
                    Entity = EntitiesEnum.Category
                }
            );

        #endregion
    }
}
