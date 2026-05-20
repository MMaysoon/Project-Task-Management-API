
namespace ProjectManagement.Application.Dtos.Pagination
{

    public class PagedList<T>
    {
        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public IList<T> Items { get; private set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;

            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            Items = items;
        }
    }
}
