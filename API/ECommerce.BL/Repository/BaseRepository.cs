using ECommerce.BLL.IRepository;
using ECommerce.DAL;
using ECommerce.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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

        public T Update(T Entity)
        {
            _context.Set<T>().Update(Entity);
            return Entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
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
            {
                return PhotoName;
            }

            return Photo;
        }

        public bool SetAvtive(bool IsActive)
        {
            return !IsActive;
        }
    }
}
