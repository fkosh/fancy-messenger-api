using System;
using System.Collections.Generic;
using System.Linq;

namespace FancyMessengerApi.Dto
{
    /// <summary>
    /// TODO process by page bad practise
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedListDto<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
 
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
 
        public PagedListDto(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
 
            AddRange(items);
        }
 
        public static PagedListDto<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
 
            return new PagedListDto<T>(items, count, pageNumber, pageSize);
        }
    }
}