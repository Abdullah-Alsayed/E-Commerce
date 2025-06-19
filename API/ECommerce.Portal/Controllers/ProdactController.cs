using ECommerce.BLL.Features.Brands.Requests;
using ECommerce.BLL.Features.Brands.Services;
using ECommerce.BLL.Features.Categories.Requests;
using ECommerce.BLL.Features.Categories.Services;
using ECommerce.BLL.Features.Collections.Requests;
using ECommerce.BLL.Features.Collections.Services;
using ECommerce.BLL.Features.Colors.Requests;
using ECommerce.BLL.Features.Colors.Services;
using ECommerce.BLL.Features.Products.Dtos;
using ECommerce.BLL.Features.Products.Requests;
using ECommerce.BLL.Features.Products.Services;
using ECommerce.BLL.Features.SubCategories.Requests;
using ECommerce.BLL.Features.SubCategories.Services;
using ECommerce.BLL.Features.Tags.Requests;
using ECommerce.BLL.Features.Tags.Services;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.PermissionsClaims;
using ECommerce.Portal.Helpers;
using ECommerce.Portal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCaching;

namespace ECommerce.API.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        private readonly IUnitService _unitService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly ITagService _tagService;
        private readonly ICollectionService _collectionService;
        private readonly IColorService _colorService;

        public ProductController(
            IProductService service,
            IUnitService unitService,
            ICategoryService categoryService,
            IBrandService brandService,
            ITagService tagService,
            ICollectionService collectionService,
            IColorService colorService
        )
        {
            _service = service;
            _unitService = unitService;
            _categoryService = categoryService;
            _brandService = brandService;
            _tagService = tagService;
            _collectionService = collectionService;
            _colorService = colorService;
        }

        [Authorize(Policy = Permissions.Products.View)]
        public IActionResult List() => View();

        #region CRUD
        [HttpPost]
        [Authorize(Policy = Permissions.Products.View)]
        public async Task<IActionResult> Table([FromBody] DataTableRequest request)
        {
            var search = request?.Search?.Value;
            var dir = request?.Order?.FirstOrDefault()?.Dir ?? Constants.Descending;
            bool isDescending = (dir == Constants.Descending);
            var columns = new List<string>
            {
                nameof(ProductDto.Code),
                nameof(ProductDto.Title),
                nameof(ProductDto.Price),
                nameof(ProductDto.ProductPhotos),
                nameof(ProductDto.Description),
                nameof(ProductDto.Brand),
                nameof(ProductDto.Unit),
                nameof(ProductDto.Category),
                nameof(ProductDto.SubCategory),
                nameof(ProductDto.IsActive),
                nameof(ProductDto.CreateAt),
            };
            string sortColumn = columns[
                request?.Order?.FirstOrDefault()?.Column ?? columns.Count - 1
            ];

            var response = await _service.GetAllAsync(
                new GetAllProductRequest
                {
                    IsDescending = isDescending,
                    SortBy = sortColumn,
                    PageSize = request?.Length ?? Constants.PageSize,
                    PageIndex = request?.PageIndex ?? Constants.PageIndex,
                    SearchFor = search,
                }
            );

            var jsonResponse = new
            {
                draw = request?.Draw ?? 0,
                recordsTotal = response?.Total ?? 0,
                recordsFiltered = response?.Total ?? 0,
                data = response?.Result.Items ?? new List<ProductDto>()
            };

            return Json(jsonResponse);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Products.Create)]
        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> Create()
        {
            var units = await _unitService.GetAllAsync(new GetAllUnitRequest { IsActive = true });
            var categories = await _categoryService.GetAllAsync(
                new GetAllCategoryRequest { IsActive = true }
            );
            var brands = await _brandService.GetAllAsync(
                new GetAllBrandRequest { IsActive = true }
            );
            var tags = await _tagService.GetAllAsync(new GetAllTagRequest { IsActive = true });

            var collections = await _collectionService.GetAllAsync(
                new GetAllCollectionRequest { IsActive = true }
            );

            var colors = await _colorService.GetAllAsync(
                new GetAllColorRequest { IsActive = true }
            );

            var viewModel = new CreateProductViewModel
            {
                Units = units?.Result?.Items ?? new(),
                Categories = categories?.Result?.Items ?? new(),
                Brands = brands?.Result?.Items ?? new(),
                Tags = tags?.Result?.Items ?? new(),
                Collections = collections?.Result?.Items ?? new(),
                Colors = colors?.Result?.Items ?? new(),
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Products.Create)]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

            try
            {
                var result = await _service.CreateAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Policy = Permissions.Products.Update)]
        public async Task<IActionResult> Update([FromForm] UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

            try
            {
                var result = await _service.UpdateAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Policy = Permissions.Products.Delete)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(DashboardHelpers.ValidationErrors(ModelState));

            try
            {
                var result = await _service.DeleteAsync(
                    new DeleteProductRequest { ID = Guid.Parse(id) }
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse { IsSuccess = false, Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<BaseResponse> GetAll([FromQuery] GetAllProductRequest request)
        {
            try
            {
                return await _service.GetAllAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        [HttpPut]
        public async Task<BaseResponse> ToggleActive([FromBody] ToggleActiveProductRequest request)
        {
            try
            {
                return await _service.ToggleActiveAsync(request);
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<BaseResponse> Get(Guid id)
        {
            try
            {
                return await _service.FindAsync(new FindProductRequest { ID = id });
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = false, Message = ex.Message };
            }
        }

        #endregion
    }
}
