using System.Collections.Generic;
using System.Linq;
using Blog.Application.Exceptions;

namespace Blog.Application.Helpers
{
    public class PagedResult<T> : List<T>, IPagedList<T>
    {
        public PagedResult(IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex < 0)
            {
                throw new BadRequestException("Page index must be greater than or equal to zero");
            }
            if (pageSize < 1)
            {
                throw new BadRequestException("Page size must be a least 1");
            }

            var total = source.Count();

            TotalCount = total;
            TotalPages = total / pageSize;
            Results = source;

            if (total % pageSize > 0)
            {
                TotalPages += 1;
            }

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageSize * pageIndex).Take(pageSize).ToList());
        }

        public PagedResult(IList<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex < 0)
            {
                throw new BadRequestException("Page index must be greater than or equal to zero");
            }
            if (pageSize < 1)
            {
                throw new BadRequestException("Page size must be a least 1");
            }

            var total = source.Count();

            TotalCount = total;
            TotalPages = total / pageSize;
            Results = source;

            if (total % pageSize > 0)
            {
                TotalPages += 1;
            }

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageIndex).Take(pageSize).ToList());
        }

        public PagedResult(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            if (pageIndex < 0)
            {
                throw new BadRequestException("Page index must be greater than or equal to zero");
            }
            if (pageSize < 1)
            {
                throw new BadRequestException("Page size must be a least 1");
            }

            TotalCount = totalCount;
            TotalPages = totalCount / pageSize;
            var enumerable = source as T[] ?? source.ToArray();
            Results = enumerable;

            if (totalCount % pageSize > 0)
            {
                TotalPages += 1;
            }

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(enumerable);
        }

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }
        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }
        public IEnumerable<T> Results { get; internal set; }
    }
}
