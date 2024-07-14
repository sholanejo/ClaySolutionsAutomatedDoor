namespace ClaySolutionsAutomatedDoor.Application.Common.Utility
{
    public class PaginatedParameter<T>
    {
        private IQueryable<T> _list;

        private int? _page;

        private int? _pageSize;

        public IQueryable<T> items
        {
            get
            {
                if (_list == null)
                {
                    return null;
                }

                return _list.Skip((page - 1) * pageSize).Take(pageSize);
            }
        }

        public int page
        {
            get
            {
                if (!_page.HasValue)
                {
                    return 1;
                }

                return _page.Value;
            }
        }

        public int pageSize
        {
            get
            {
                if (!_pageSize.HasValue)
                {
                    if (_list != null)
                    {
                        return _list.Count();
                    }

                    return 0;
                }

                return _pageSize.Value;
            }
        }

        public int totalItemCount
        {
            get
            {
                if (_list != null)
                {
                    return _list.Count();
                }

                return 0;
            }
        }

        public PaginatedParameter(IQueryable<T> list, int? page = null, int? pageSize = null)
        {
            _list = list;
            _page = page;
            _pageSize = pageSize;
        }
    }
}
