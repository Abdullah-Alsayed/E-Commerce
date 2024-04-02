using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using ECommerce.BLL.Features.Roles.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Request;
using ECommerce.Core;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace ECommerce.BLL.Repository;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<Role> _roleManager;

    public RoleRepository(ApplicationDbContext context, RoleManager<Role> roleManager)
        : base(context)
    {
        _context = context;
        _roleManager = roleManager;
    }

    public override async Task<List<Role>> GetAllAsync(BaseGridRequest request)
    {
        try
        {
            var result = new List<Role>();
            var query = _context.Roles.Where(x => !x.IsDeleted);
            if (!string.IsNullOrEmpty(request.SortBy))
                query = OrderByDynamic(query, request.SortBy, request.IsDescending);

            if (!string.IsNullOrEmpty(request.SearchFor) && !string.IsNullOrEmpty(request.SearchBy))
                query = SearchDynamic(query, request.SearchBy, request.SearchFor);

            var total = await query.CountAsync();
            if (total > 0)
            {
                var skippedPages = request.PageSize * request.PageIndex;
                result = await query
                    .Skip(skippedPages)
                    .Take(request.PageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }

            return result;
        }
        catch (Exception ex)
        {
            await _context.ErrorLogs.AddAsync(
                new ErrorLog
                {
                    Source = ex.Source,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Operation = DAL.Enums.OperationTypeEnum.GetAll,
                    Entity = DAL.Enums.EntitiesEnum.Role
                }
            );
            await _context.SaveChangesAsync();
            return new List<Role>();
        }
    }

    public async Task<int> UpdateRoleClaimsAsync(UpdateRoleClaimsRequest request)
    {
        try
        {
            var modifyRows = 0;
            string[] parts = new string[3];
            var requestClaims = request.Claims.Distinct().ToList();
            var roleClaims = await _context
                .RoleClaims.Where(role => role.RoleId == request.RoleID)
                .ToListAsync();

            // Find claims to delete
            var claimsToDelete = roleClaims
                .Where(rc => !requestClaims.Contains(rc.ClaimValue))
                .ToList();

            // Find new claims
            var newClaimsToAdd = requestClaims
                .Except(roleClaims.Select(rc => rc.ClaimValue))
                .ToList();

            if (claimsToDelete != null && claimsToDelete.Any())
                foreach (var claim in claimsToDelete)
                {
                    _context.RoleClaims.Remove(claim);
                    modifyRows++;
                }

            if (newClaimsToAdd != null && newClaimsToAdd.Any())
                foreach (var claim in newClaimsToAdd)
                {
                    parts = claim.Split('.');
                    await _context.RoleClaims.AddAsync(
                        new RoleClaims
                        {
                            ClaimType = Constants.Permission,
                            ClaimValue = claim,
                            Module = parts[1],
                            Operation = parts[2],
                            RoleId = request.RoleID,
                        }
                    );
                    modifyRows++;
                }

            return modifyRows;
        }
        catch (Exception ex)
        {
            await _context.ErrorLogs.AddAsync(
                new ErrorLog
                {
                    Source = ex.Source,
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    Operation = DAL.Enums.OperationTypeEnum.UpdateClaims,
                    Entity = DAL.Enums.EntitiesEnum.Role
                }
            );
            return 1;
        }
    }

    private IQueryable<Q> OrderByDynamic<Q>(IQueryable<Q> query, string orderByColumn, bool isDesc)
    {
        var QType = typeof(Q);
        orderByColumn =
            orderByColumn != "ID"
                ? char.ToUpper(orderByColumn[0]) + orderByColumn.Substring(1)
                : "Name";
        // Dynamically creates a call like this: query.OrderBy(p => p.SortColumn)
        var parameter = Expression.Parameter(QType, "p");
        Expression resultExpression = null;
        var property = QType.GetProperty(orderByColumn ?? "Name");
        // this is the part p.SortColumn
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        // this is the part p => p.SortColumn
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        // finally, call the "OrderBy" / "OrderByDescending" method with the order by lamba expression
        resultExpression = Expression.Call(
            typeof(Queryable),
            isDesc ? "OrderByDescending" : "OrderBy",
            new Type[] { QType, property.PropertyType },
            query.Expression,
            Expression.Quote(orderByExpression)
        );

        return query.Provider.CreateQuery<Q>(resultExpression);
    }
}
