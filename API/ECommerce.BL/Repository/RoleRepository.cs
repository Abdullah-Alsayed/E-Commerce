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
    private readonly UserManager<User> _userManager;

    public RoleRepository(
        ApplicationDbContext context,
        RoleManager<Role> roleManager,
        UserManager<User> userManager
    )
        : base(context)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<Role> FindByName(string name)
    {
        return await _roleManager.FindByNameAsync(name);
    }

    public async Task<Role> FindById(string id)
    {
        return await _roleManager.FindByIdAsync(id);
    }

    public async Task<bool> AddUserToRoleAsync(AddUserToRoleRequest request)
    {
        try
        {
            var response = false;
            var user = await _userManager.FindByIdAsync(request.UserID);
            var roles = await _roleManager
                .Roles.Where(x => request.RoleIDs.Contains(x.Id))
                .ToListAsync();
            var rolesName = roles.Select(x => x.Name);
            var result = await _userManager.AddToRolesAsync(user, rolesName);
            if (result.Succeeded)
                response = true;

            return response;
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
            return false;
        }
    }

    public async Task<bool> UpdateUserRoleAsync(UpdateUserRoleRequest request)
    {
        try
        {
            var response = false;
            var resultNewRole = new IdentityResult();
            var resultRoleToDelete = new IdentityResult();
            var user = await _userManager.FindByIdAsync(request.UserID);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = await _roleManager
                .Roles.Where(x => request.RoleIDs.Contains(x.Id))
                .ToListAsync();
            var rolesName = roles.Select(x => x.Name);

            var newRoles = rolesName.Where(x => !userRoles.Contains(x));

            // Find Role to delete
            var roleToDelete = userRoles.Where(role => !rolesName.Contains(role));

            // Find new Role
            var newRoleToAdd = rolesName.Except(userRoles.Select(role => role));

            if (newRoleToAdd != null && newRoleToAdd.Any())
                resultNewRole = await _userManager.AddToRolesAsync(user, newRoleToAdd);

            if (roleToDelete != null && roleToDelete.Any())
                resultRoleToDelete = await _userManager.RemoveFromRolesAsync(user, roleToDelete);

            if (resultNewRole.Errors.Count() == 0 && resultRoleToDelete.Errors.Count() == 0)
                response = true;

            return response;
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
            return false;
        }
    }

    public override async Task<List<Role>> GetAllAsync(
        BaseGridRequest request,
        List<string> Includes = null
    )
    {
        try
        {
            var result = new List<Role>();
            var query = _context.Roles.Where(x => !x.IsDeleted);
            query = ApplyDynamicQuery(request, query);

            var total = await query.CountAsync();
            if (total > 0)
            {
                query = ApplyPagination(request, query);
                result = await query.AsNoTracking().ToListAsync();
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

    public async Task<int> UpdateUserClaimsAsync(UpdateUserClaimsRequest request)
    {
        try
        {
            var modifyRows = 0;
            string[] parts = new string[3];
            var requestClaims = request.Claims.Distinct().ToList();
            var userClaims = await _context
                .UserClaims.Where(role => role.UserId == request.UserID)
                .ToListAsync();

            // Find claims to delete
            var claimsToDelete = userClaims
                .Where(rc => !requestClaims.Contains(rc.ClaimValue))
                .ToList();

            // Find new claims
            var newClaimsToAdd = requestClaims
                .Except(userClaims.Select(rc => rc.ClaimValue))
                .ToList();

            if (claimsToDelete != null && claimsToDelete.Any())
                foreach (var claim in claimsToDelete)
                {
                    _context.UserClaims.Remove(claim);
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
                            RoleId = request.UserID,
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
                    Entity = DAL.Enums.EntitiesEnum.User
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
