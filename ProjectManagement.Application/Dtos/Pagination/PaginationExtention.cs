using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.Application.Dtos.Pagination
{
    public static class PaginationExtension
    {
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source,int pageNumber,int pageSize) where T : class
        {
            var count = await source.CountAsync();

            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
