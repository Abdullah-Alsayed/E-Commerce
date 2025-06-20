﻿using Azure;
using ECommerce.BLL.Futures.Governorates.Requests;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Request;
using ECommerce.BLL.Response;
using ECommerce.DAL;
using ECommerce.DAL.Entity;
using ECommerce.Helpers;
using ECommerce.Services;
using MailKit.Search;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.BLL.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        private readonly Applicationdbcontext _context;
        private readonly IHostingEnvironment hosting;

        public BaseRepository(Applicationdbcontext context, IHostingEnvironment hosting)
        {
            _context = context;
            this.hosting = hosting;
        }

        public async Task<T> AddaAync(T Entity)
        {
            await _context.Set<T>().AddAsync(Entity);
            return Entity;
        }

        public async Task<IEnumerable<T>> AddaRangeAync(IEnumerable<T> Entitys)
        {
            await _context.Set<T>().AddRangeAsync(Entitys);
            return Entitys;
        }

        public bool Delete(T Entity)
        {
            if (Entity == null)
                return false;
            else
                _context.Set<T>().Remove(Entity);
            return true;
        }

        public async Task<T> FindAsync(int ID)
        {
            var Result = await _context.Set<T>().FindAsync(ID);
            return Result;
        }

        public async Task<T> FindAsync(Guid ID)
        {
            var Result = await _context.Set<T>().FindAsync(ID);
            return Result;
        }

        public T Update(T Entity)
        {
            _context.Set<T>().Update(Entity);
            return Entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(BaseGridRequest request)
        {
            try
            {
                var result = new List<T>();
                IQueryable<T> query = _context.Set<T>();

                query = IsDeletedDynamic(query, request.IsDeleted);

                if (!string.IsNullOrEmpty(request.SortBy))
                    query = OrderByDynamic(query, request.SortBy, request.IsDescending);
                if (
                    !string.IsNullOrEmpty(request.SearchFor)
                    && !string.IsNullOrEmpty(request.SearchBy)
                )
                    query = SearchDynamic(query, request.SearchBy, request.SearchFor);

                var total = await query.CountAsync();
                if (total > 0)
                {
                    var skipedPages = request.PageSize * request.PageIndex;
                    result = await query.Skip(skipedPages).Take(request.PageSize).ToListAsync();
                }

                return result;
            }
            catch
            {
                return new List<T>();
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync(string[] Includes = null)
        {
            IQueryable<T> Query = _context.Set<T>();
            foreach (var incluse in Includes)
                Query = Query.Include(incluse);

            return await Query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> Criteria,
            string[] Includes = null
        )
        {
            IQueryable<T> Query = _context.Set<T>();
            foreach (var incluse in Includes)
                Query = Query.Include(incluse);

            return await Query.Where(Criteria).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> Criteria,
            string[] Includes = null,
            Expression<Func<T, object>> orderBy = null,
            string orderByDirection = Constants.OrderBY.Ascending
        )
        {
            IQueryable<T> Query = _context.Set<T>();
            if (orderBy != null)
            {
                if (orderByDirection == Constants.OrderBY.Ascending)
                    Query = Query.OrderBy(orderBy);
                else
                    Query = Query.OrderByDescending(orderBy);
            }
            foreach (var incluse in Includes)
                Query = Query.Include(incluse);

            return await Query.Where(Criteria).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            string[] Includes = null,
            Expression<Func<T, object>> orderBy = null,
            string orderByDirection = Constants.OrderBY.Ascending
        )
        {
            IQueryable<T> Query = _context.Set<T>();
            if (orderBy != null)
            {
                if (orderByDirection == Constants.OrderBY.Ascending)
                    Query = Query.OrderBy(orderBy);
                else
                    Query = Query.OrderByDescending(orderBy);
            }
            foreach (var incluse in Includes)
                Query = Query.Include(incluse);

            return await Query.ToListAsync();
        }

        public async Task<T> GetItemAsync(
            Expression<Func<T, bool>> Criteria,
            string[] Includes = null
        )
        {
            IQueryable<T> Query = _context.Set<T>();
            if (Includes != null)
            {
                foreach (var incluse in Includes)
                    Query = Query.Include(incluse);
            }
            return await Query.FirstOrDefaultAsync(Criteria);
        }

        public async Task<string> UplodPhoto(
            IFormFile File,
            string FolderName,
            string PhotoName = null
        )
        {
            string Photo = null;
            if (File != null)
            {
                var Extansion = Path.GetExtension(File.FileName);
                var GuId = Guid.NewGuid().ToString();
                Photo = GuId + Extansion;
                var Filepath = Path.Combine(hosting.ContentRootPath, "Images", FolderName, Photo);
                await File.CopyToAsync(new FileStream(Filepath, FileMode.Create));
            }
            if (PhotoName != null && File != null)
            {
                var Filepath = Path.Combine(
                    hosting.ContentRootPath,
                    "Images",
                    FolderName,
                    PhotoName
                );
                System.IO.File.Delete(Filepath);
            }
            if (PhotoName != null && File == null)
                return PhotoName;

            return Photo;
        }

        public bool ToggleAvtive(bool IsActive)
        {
            return !IsActive;
        }

        private static IQueryable<Q> OrderByDynamic<Q>(
            IQueryable<Q> query,
            string orderByColumn,
            bool isDesc
        )
        {
            var QType = typeof(Q);
            orderByColumn = char.ToUpper(orderByColumn[0]) + orderByColumn.Substring(1);
            // Dynamically creates a call like this: query.OrderBy(p => p.SortColumn)
            var parameter = Expression.Parameter(QType, "p");
            Expression resultExpression = null;
            var property = QType.GetProperty(orderByColumn ?? "ID");
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

        public IQueryable<T> SearchDynamic(
            IQueryable<T> query,
            string propertyName,
            string searchTerm
        )
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(searchTerm))
                return query;
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

            MemberExpression property = Expression.PropertyOrField(parameter, propertyName);

            MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
            Expression toLowerExpression = Expression.Call(property, toLowerMethod);

            ConstantExpression constantTerm = Expression.Constant(searchTerm.ToLower());

            MethodInfo containsMethod = typeof(string).GetMethod(
                "Contains",
                new[] { typeof(string) }
            );
            Expression containsExpression = Expression.Call(
                toLowerExpression,
                containsMethod,
                constantTerm
            );

            Expression<Func<T, bool>> predicate = Expression.Lambda<Func<T, bool>>(
                containsExpression,
                parameter
            );

            return query.Where(predicate);
        }

        public IQueryable<T> IsDeletedDynamic(IQueryable<T> query, bool propertyValue)
        {
            // Create a parameter expression
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

            // Create property access expression
            MemberExpression property = Expression.PropertyOrField(parameter, "IsDeleted");

            // Create constant expression for dynamic value
            ConstantExpression value = Expression.Constant(propertyValue);

            // Create equality expression: x.Property == propertyValue
            BinaryExpression equality = Expression.Equal(property, value);

            // Create lambda expression
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(
                equality,
                parameter
            );
            // Now, use the filter function to filter entities
            return query.Where(lambda);
        }

        public List<string> SearchEntity()
        {
            Type entityType = typeof(T);
            var stringProperties = entityType
                .GetProperties()
                .Where(prop => prop.PropertyType == typeof(string))
                .Select(x => x.Name)
                .ToList();

            return stringProperties;
        }
    }
}
