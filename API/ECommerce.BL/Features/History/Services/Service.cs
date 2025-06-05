using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Histories.Dtos;
using ECommerce.BLL.Features.Histories.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Response;
using ECommerce.Core;
using ECommerce.Core.Services.User;
using ECommerce.DAL.Entity;
using Microsoft.Extensions.Localization;
using static ECommerce.Core.Constants;

namespace ECommerce.BLL.Features.Histories.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IStringLocalizer<HistoryService> _localizer;

        private Guid _userId = Guid.Empty;
        private string _userName = Constants.System;
        private string _lang = Constants.Languages.Ar;

        public HistoryService(
            IUnitOfWork unitOfWork,
            IStringLocalizer<HistoryService> localizer,
            IUserContext userContext
        )
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _userContext = userContext;

            #region Get User Data From Token
            _userId = _userContext.UserId.Value;

            _userName = _userContext.UserName.Value;

            _lang = _userContext.Language.Value;
            #endregion
        }

        public async Task<BaseResponse<BaseGridResponse<List<HistoryDto>>>> GetAllAsync(
            GetAllHistoryRequest request
        )
        {
            try
            {
                var result = await _unitOfWork.History.GetAllAsync(
                    request,
                    new List<string> { nameof(User) }
                );

                var response = result
                    .list.Select(x => new HistoryDto
                    {
                        Id = x.ID,
                        Action = x.Action,
                        Entity = x.Entity,
                        UserName = $"{x.User.FirstName} {x.User.FirstName}",
                        CreateAt = x.Date,
                    })
                    .ToList();

                return new BaseResponse<BaseGridResponse<List<HistoryDto>>>
                {
                    IsSuccess = true,
                    Message = _localizer[MessageKeys.Success].ToString(),
                    Total = result.count,
                    Result = new BaseGridResponse<List<HistoryDto>>
                    {
                        Items = response,
                        Total = result.count,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<BaseGridResponse<List<HistoryDto>>>
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
