// Ignore Spelling: BLL

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using ECommerce.BLL.IRepository;
using ECommerce.BLL.Request;
using ECommerce.Core;
using ECommerce.DAL;
using ECommerce.DAL.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.BLL.Repository
{
    public class BaseRepository<T> : IBaseRepository<T>
        where T : class
    {
        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> AddAsync(T entity, Guid userId)
        {
            if (entity is IBaseEntity auditableEntity)
            {
                auditableEntity.CreateAt = DateTime.UtcNow;
                auditableEntity.CreateBy = userId;
            }

            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public T Update(T entity, Guid userId)
        {
            if (entity is IBaseEntity auditableEntity)
            {
                auditableEntity.ModifyAt = DateTime.UtcNow;
                auditableEntity.ModifyBy = userId;
            }

            _context.Set<T>().Update(entity);
            return entity;
        }

        public T Delete(T entity, Guid userId)
        {
            if (entity is IBaseEntity auditableEntity)
            {
                auditableEntity.DeletedAt = DateTime.UtcNow;
                auditableEntity.DeletedBy = userId;
                auditableEntity.IsDeleted = true;
            }

            return entity;
        }

        public T ToggleActive(T entity, Guid userId)
        {
            if (entity is IBaseEntity auditableEntity)
            {
                auditableEntity.ModifyAt = DateTime.UtcNow;
                auditableEntity.ModifyBy = userId;
                auditableEntity.IsActive = !auditableEntity.IsActive;
            }

            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entitys)
        {
            await _context.Set<T>().AddRangeAsync(entitys);
            return entitys;
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

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public virtual async Task<(List<T> list, int count)> GetAllAsync(
            BaseGridRequest request,
            List<string> Includes = null
        )
        {
            try
            {
                var result = new List<T>();
                IQueryable<T> query = _context.Set<T>();
                if (Includes != null)
                    foreach (var include in Includes)
                        query = query.Include(include);

                query = ApplyDynamicQuery(request, query);

                var total = await query.CountAsync();
                if (total > 0)
                {
                    query = ApplyPagination(request, query);
                    result = await query.AsNoTracking().ToListAsync();
                }

                return (result, total);
            }
            catch (Exception ex)
            {
                return (new List<T>(), 0);
            }
        }

        public async Task<(IEnumerable<T> list, int count)> GetAllAsync(
            BaseGridRequest request,
            Expression<Func<T, bool>> criteria,
            IEnumerable<string> Includes = null
        )
        {
            try
            {
                var result = new List<T>();
                IQueryable<T> query = _context.Set<T>();
                if (Includes != null)
                    foreach (var incluse in Includes)
                        query = query.Include(incluse);

                query = query.Where(criteria);

                query = ApplyDynamicQuery(request, query);

                var total = await query.CountAsync();
                if (total > 0)
                {
                    query = ApplyPagination(request, query);
                    result = await query.AsNoTracking().ToListAsync();
                }
                return (result, total);
            }
            catch (Exception ex)
            {
                return (new List<T>(), 0);
            }
        }

        public static IQueryable<T> ApplyPagination(BaseGridRequest request, IQueryable<T> query)
        {
            var skipedPages = request.PageSize * request.PageIndex;
            return query.Skip(skipedPages).Take(request.PageSize);
        }

        public IQueryable<T> ApplyDynamicQuery(BaseGridRequest request, IQueryable<T> query)
        {
            query = FilterDynamic(query, request.IsDeleted, nameof(request.IsDeleted));

            if (request.IsActive.HasValue)
                query = FilterDynamic(query, request.IsActive.Value, nameof(request.IsActive));

            if (!string.IsNullOrEmpty(request.SortBy))
                query = OrderByDynamic(query, request.SortBy, request.IsDescending);

            if (!string.IsNullOrEmpty(request.SearchFor) && !string.IsNullOrEmpty(request.SearchBy))
                query = SearchDynamic(query, request.SearchBy, request.SearchFor);

            return query;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string[] Includes = null)
        {
            IQueryable<T> Query = _context.Set<T>();

            foreach (var include in Includes)
                Query = Query.Include(include);

            return await Query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> Criteria,
            string[] Includes = null
        )
        {
            IQueryable<T> Query = _context.Set<T>();
            if (Includes != null)
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
            if (Includes != null)
            {
                foreach (var incluse in Includes)
                    Query = Query.Include(incluse);
            }

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
            if (Includes != null)
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

        public async Task<string> UploadPhotoAsync(
            IFormFile file,
            string folderName,
            string photoName = null
        )
        {
            string fullPath = Constants.DefaultPhotos.Default;
            try
            {
                if (file != null)
                {
                    // Get file extension
                    var extension = Path.GetExtension(file.FileName);
                    var guid = Guid.NewGuid().ToString();
                    var fileName = guid + extension;

                    // Get the absolute path for saving in wwwroot
                    var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var directoryPath = Path.Combine(wwwRootPath, "Images", folderName);

                    // Ensure directory exists
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    // Full file path
                    fullPath = Path.Combine(directoryPath, fileName);

                    // Save the file
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    // Convert absolute path to relative path for DB storage
                    fullPath = $"/Images/{folderName}/{fileName}";
                }

                // Delete old photo if provided
                if (
                    !string.IsNullOrEmpty(photoName)
                    && photoName != Constants.DefaultPhotos.Default
                    && file != null
                )
                {
                    var oldFilePath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        photoName.TrimStart('/')
                    );

                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }

                // If no new file is uploaded but old photo exists, return the old photo
                if (
                    !string.IsNullOrEmpty(photoName)
                    && photoName != Constants.DefaultPhotos.Default
                    && file == null
                )
                    return photoName;

                return fullPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Constants.DefaultPhotos.Default; // Return default image on error
            }
        }

        public async Task<string> UploadPhotoAsync(Stream file, string folderName)
        {
            try
            {
                if (file != null)
                {
                    var guid = Guid.NewGuid().ToString();
                    var extension = ".png"; // Assuming avatar is always PNG
                    var fileName = guid + extension;

                    // Get wwwroot path
                    var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var directoryPath = Path.Combine(wwwRootPath, "Images", folderName);

                    // Ensure directory exists
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    // Create full path
                    var fullPath = Path.Combine(directoryPath, fileName);

                    // Save file
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    // Return relative path (for storing in DB)
                    return $"/Images/{folderName}/{fileName}";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // Return default image if an error occurs
            return "/Images/default.png";
        }

        public async Task<List<string>> UploadPhotos(
            List<IFormFile> files,
            string FolderName,
            List<string> ImgNames = null
        )
        {
            var photos = new List<string>();
            var currentPhoto = string.Empty;
            var index = 0;
            foreach (var file in files)
            {
                currentPhoto = await UploadPhotoAsync(
                    file,
                    FolderName,
                    ImgNames != null ? ImgNames[index] : null
                );
                photos.Add(currentPhoto);
                index++;
            }

            return photos;
        }

        public async Task DeleteFile(string fullPath)
        {
            try
            {
                string directoryPath = System.IO.Path.GetDirectoryName(fullPath);

                // Check if the directory exists
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    Console.WriteLine("Directory does not exist.");
                    return;
                }

                // Check if the file exists
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                    Console.WriteLine("File deleted successfully.");
                }
                else
                    Console.WriteLine("File does not exist.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public bool ToggleActive(bool IsActive)
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

        public IQueryable<T> FilterDynamic(
            IQueryable<T> query,
            bool propertyValue,
            string propertyName
        )
        {
            // Create a parameter expression
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");

            // Create property access expression
            MemberExpression property = Expression.PropertyOrField(parameter, propertyName);

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

        public async Task<T> FirstAsync()
        {
            return await _context.Set<T>().FirstOrDefaultAsync();
        }

        public async Task<T> FirstAsync(Expression<Func<T, bool>> Criteria)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(Criteria);
        }

        public async Task<T> FirstAsync(
            Expression<Func<T, bool>> Criteria,
            List<string> Includes = null
        )
        {
            IQueryable<T> Query = _context.Set<T>();
            if (Includes != null)
                foreach (var include in Includes)
                    Query = Query.Include(include);

            return await Query.FirstOrDefaultAsync(Criteria);
        }
    }
}
