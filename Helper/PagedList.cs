using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BriefResume.Helper
{
    public class PagedList<T>:List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPage { get; private set; }
        public int PageSize { get; set; }
        public int TotalItemCount { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPage;
        public PagedList(List<T> items,int count,int pageNumber,int pageSize)
        {
            TotalItemCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
        //下方代码实属惊艳到我,以后也要好好学设计模式,聪聪!

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> result,int currentPage,int pageSize)//工厂模式
        {
            var count = result.Count();
            var items = await result.Skip((currentPage - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, currentPage, pageSize);
        }

    }
}
