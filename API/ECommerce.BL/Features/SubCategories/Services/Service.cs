using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Categories.Dtos;
using ECommerce.BLL.Features.SubCategories.Dtos;
using ECommerce.BLL.Features.SubCategories.Requests;
using ECommerce.BLL.Response;
using ECommerce.BLL.UnitOfWork;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using ECommerce.DAL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.SubCategories.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IStringLocalizer<SubCategoryService> _localizer;
        private readonly IHostEnvironment _environment;

        private Guid _userId = Guid.Empty;
        private string _userName = Constants.System;
        private string _lang = Constants.Languages.Ar;

        public SubCategoryService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<SubCategoryService> localizer,
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
                cfg.CreateMap<SubCategory, SubCategoryDto>()
                    .ForMember(
                        dest => dest.Name,
                        opt =>
                            opt.MapFrom(src =>
                                _lang == Constants.Languages.Ar ? src.NameAR : src.NameEN
                            )
                    )
                    .ReverseMap();
                cfg.CreateMap<Category, CategoryDto>()
                    .ForMember(
                        dest => dest.Name,
                        opt =>
                            opt.MapFrom(src =>
                                _lang == Constants.Languages.Ar ? src.NameAR : src.NameEN
                            )
                    )
                    .ReverseMap();
                cfg.CreateMap<SubCategory, CreateSubCategoryRequest>().ReverseMap();
                cfg.CreateMap<SubCategory, UpdateSubCategoryRequest>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion initilize mapper

            #region Get User Data From Token
            _userId = _userContext.UserId.Value;

            _userName = _userContext.UserName.Value;

            _lang = _userContext.Language.Value;
            #endregion
        }

        public async Task<BaseResponse> FindAsync(FindSubCategoryRequest request)
        {
            try
            {
                var SubCategory = await _unitOfWork.ProductModule.SubCategory.FindAsync(request.ID);
                var result = _mapper.Map<SubCategoryDto>(SubCategory);
                return new BaseResponse<SubCategoryDto>
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
                    EntitiesEnum.SubCategory
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse<BaseGridResponse<List<SubCategoryDto>>>> GetAllAsync(
            GetAllSubCategoryRequest request
        )
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? _lang == Languages.Ar
                        ? nameof(SubCategory.NameAR)
                        : nameof(SubCategory.NameEN)
                    : request.SearchBy;

                var includes = new List<string>();
                if (request.IncludeReferences.HasValue && request.IncludeReferences.Value)
                    includes.Add(nameof(Category));

                var result =
                    request.CategoryId.HasValue && request.CategoryId.Value != Guid.Empty
                        ? await _unitOfWork.ProductModule.SubCategory.GetAllAsync(
                            request,
                            x => x.CategoryID == request.CategoryId.Value,
                            includes
                        )
                        : await _unitOfWork.ProductModule.SubCategory.GetAllAsync(
                            request,
                            includes
                        );

                var response = _mapper.Map<List<SubCategoryDto>>(result.list);
                return new BaseResponse<BaseGridResponse<List<SubCategoryDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Total = response != null ? result.count : 0,
                    Result = new BaseGridResponse<List<SubCategoryDto>>
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
                    EntitiesEnum.SubCategory
                );
                return new BaseResponse<BaseGridResponse<List<SubCategoryDto>>>
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> CreateAsync(CreateSubCategoryRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var SubCategory = _mapper.Map<SubCategory>(request);
                SubCategory = await _unitOfWork.ProductModule.SubCategory.AddAsync(
                    SubCategory,
                    _userId
                );
                SubCategory.PhotoPath =
                    await _unitOfWork.ProductModule.SubCategory.UploadPhotoAsync(
                        request.FormFile,
                        Constants.PhotoFolder.SubCategorys
                    );
                var result = _mapper.Map<SubCategoryDto>(SubCategory);

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
                    return new BaseResponse<SubCategoryDto>
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
                    EntitiesEnum.SubCategory
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UpdateAsync(UpdateSubCategoryRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var SubCategory = await _unitOfWork.ProductModule.SubCategory.FindAsync(request.ID);
                _mapper.Map(request, SubCategory);
                SubCategory.PhotoPath =
                    await _unitOfWork.ProductModule.SubCategory.UploadPhotoAsync(
                        request.FormFile,
                        Constants.PhotoFolder.SubCategorys,
                        SubCategory.PhotoPath
                    );
                _unitOfWork.ProductModule.SubCategory.Update(SubCategory, _userId);
                var result = _mapper.Map<SubCategoryDto>(SubCategory);

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
                    return new BaseResponse<SubCategoryDto>
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
                    EntitiesEnum.SubCategory
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> DeleteAsync(DeleteSubCategoryRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var SubCategory = await _unitOfWork.ProductModule.SubCategory.FindAsync(request.ID);
                _unitOfWork.ProductModule.SubCategory.Delete(SubCategory, _userId);
                var result = _mapper.Map<SubCategoryDto>(SubCategory);

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
                    return new BaseResponse<SubCategoryDto>
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
                    EntitiesEnum.SubCategory
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> ToggleActiveAsync(ToggleActiveSubCategoryRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var SubCategory = await _unitOfWork.ProductModule.SubCategory.FindAsync(request.ID);
                _unitOfWork.ProductModule.SubCategory.ToggleActive(SubCategory, _userId);
                var result = _mapper.Map<SubCategoryDto>(SubCategory);

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
                    return new BaseResponse<SubCategoryDto>
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
                    EntitiesEnum.SubCategory
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
                var result = _unitOfWork.ProductModule.SubCategory.SearchEntity();
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
                    EntitiesEnum.SubCategory
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
                    Entity = EntitiesEnum.SubCategory
                }
            );

        private async Task LogHistory(OperationTypeEnum action) =>
            await _unitOfWork.ContentModule.History.AddAsync(
                new History
                {
                    UserID = _userId,
                    Action = action,
                    Entity = EntitiesEnum.SubCategory
                },
                _userId
            );

        #endregion
    }
}
