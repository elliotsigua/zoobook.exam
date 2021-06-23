using System;

namespace Zoobook.Shared
{
    public class Paging : IPaging
    {
        private readonly int _page;
        private readonly int _pageSize;
        public Paging() { }

        public Paging(int page, int pageSize)
        {
            _page = page;
            _pageSize = pageSize;
        }
        public int Page => Math.Max(0, _page);
        public int PageSize => _pageSize <= 0 ? Constants.MaximumPageSize : _pageSize;
    }
}
