namespace dotNetMentoringProgram_WebApi
{
    public class QueryParamenters
    {
        public class QueryParameters
        {
            private int? _pageNumber = 1;
            private int? _pageSize = 10;

            public int? PageNumber
            {
                get => _pageNumber;
                set => _pageNumber = value > 0 ? value : 0;
            }

            public int? PageSize
            {
                get => _pageSize;
                set => _pageSize = value > 0 ? value : _pageSize;
            }

            public int? CategoryId { get; set; } = null;
        }
    }
}
