using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.BLL.Features.Colors.Dtos;
using ECommerce.BLL.Features.Products.Dtos;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.BLL.Features.Sizes.Dtos;
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

namespace ECommerce.BLL.Features.Products.Services
{
    public class ProductService : IProductService
    {
        IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IStringLocalizer<ProductService> _localizer;
        private readonly IHostEnvironment _environment;

        private Guid _userId = Guid.Empty;
        private string _userName = Constants.System;
        private string _lang = Languages.Ar;

        public ProductService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<ProductService> localizer,
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
                cfg.CreateMap<Product, ProductDto>().ReverseMap();
                cfg.CreateMap<Product, CreateProductRequest>().ReverseMap();
                cfg.CreateMap<Product, UpdateProductRequest>().ReverseMap();
                cfg.CreateMap<Color, ColorDto>().ReverseMap();
                cfg.CreateMap<Size, SizeDto>().ReverseMap();
            });
            _mapper = new Mapper(config);
            #endregion initialize mapper

            #region Get User Data From Token
            _userId = _userContext.UserId.Value;

            _userName = _userContext.UserName.Value;

            _lang = _userContext.Language.Value;
            #endregion
        }

        public async Task<BaseResponse> FindAsync(FindProductRequest request)
        {
            try
            {
                var Product = await _unitOfWork.ProductModule.Product.FindAsync(request.ID);
                var result = _mapper.Map<ProductDto>(Product);
                return new BaseResponse<ProductDto>
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
                    EntitiesEnum.Product
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> GetProductItems(GetProductItemsRequest request)
        {
            try
            {
                var Product = await _unitOfWork.ProductModule.Product.GetProductItemAsync(
                    request.ID
                );
                var productDto = _mapper.Map<ProductDto>(Product);
                var result = GetProductItemsDto(Product);
                return new BaseResponse<ProductItemDto>
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
                    EntitiesEnum.Product
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> GetAllAsync(GetAllProductRequest request)
        {
            try
            {
                request.SearchBy = string.IsNullOrEmpty(request.SearchBy)
                    ? nameof(Product.Title)
                    : request.SearchBy;

                var result = await _unitOfWork.ProductModule.Product.GetAllAsync(request);
                var response = _mapper.Map<List<ProductDto>>(result.list);
                return new BaseResponse<BaseGridResponse<List<ProductDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Total = response != null ? result.count : 0,
                    Result = new BaseGridResponse<List<ProductDto>>
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
                    EntitiesEnum.Product
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> CreateAsync(CreateProductRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var Product = _mapper.Map<Product>(request);
                Product = await _unitOfWork.ProductModule.Product.AddAsync(Product, _userId);
                Product.ProductPhotos = await _unitOfWork.ProductModule.Product.UploadPhotos(
                    request.FormFiles,
                    PhotoFolder.Products
                );
                var result = _mapper.Map<ProductDto>(Product);
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
                    return new BaseResponse<ProductDto>
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
                    EntitiesEnum.Product
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> UpdateAsync(UpdateProductRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var product = await _unitOfWork.ProductModule.Product.FindAsync(request.ID);
                _mapper.Map(request, product);
                product.ProductPhotos = await _unitOfWork.ProductModule.Product.UploadPhotos(
                    request.FormFiles,
                    PhotoFolder.Products,
                    product.ProductPhotos
                );
                _unitOfWork.ProductModule.Product.Update(product, _userId);
                var result = _mapper.Map<ProductDto>(product);
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
                    return new BaseResponse<ProductDto>
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
                    EntitiesEnum.Product
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> DeleteAsync(DeleteProductRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var product = await _unitOfWork.ProductModule.Product.FindAsync(request.ID);
                _unitOfWork.ProductModule.Product.Delete(product, _userId);
                var result = _mapper.Map<ProductDto>(product);

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
                    return new BaseResponse<ProductDto>
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
                    EntitiesEnum.Product
                );
                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = _localizer[MessageKeys.Fail].ToString()
                };
            }
        }

        public async Task<BaseResponse> ToggleActivesAsync(ToggleActiveProductRequest request)
        {
            using var transaction = await _unitOfWork.Context.Database.BeginTransactionAsync();
            var modifyRows = 0;
            try
            {
                var product = await _unitOfWork.ProductModule.Product.FindAsync(request.ID);
                _unitOfWork.ProductModule.Product.Update(product, _userId);
                product.IsActive = !product.IsActive;
                var result = _mapper.Map<ProductDto>(product);
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
                    return new BaseResponse<ProductDto>
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
                    EntitiesEnum.Product
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
                var result = _unitOfWork.ProductModule.Product.SearchEntity();
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
                    EntitiesEnum.Product
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
                    Entity = EntitiesEnum.Product
                }
            );

        private async Task LogHistory(OperationTypeEnum action) =>
            await _unitOfWork.ContentModule.History.AddAsync(
                new History
                {
                    UserID = _userId,
                    Action = action,
                    Entity = EntitiesEnum.Product
                },
                _userId
            );

        private ProductItemDto GetProductItemsDto(Product product)
        {
            var result = new ProductItemDto();

            result.Product = _mapper.Map<ProductDto>(product);
            result.ProductSizes = product
                .ProductSizes.Select(size => new ProductSizeDto
                {
                    Quantity = size.Quantity,
                    Size = _mapper.Map<SizeDto>(size.Size)
                })
                .ToList();

            result.ProductColors = product
                .ProductColors.Select(color => new ProductColorDto
                {
                    Quantity = color.Quantity,
                    Color = _mapper.Map<ColorDto>(color.Color)
                })
                .ToList();

            return result;
        }

        #endregion
    }
}
