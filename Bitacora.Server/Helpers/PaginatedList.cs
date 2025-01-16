using System;
using System.Collections.Generic;
using System.Linq;

namespace Bitacora.Helpers
{
    public class PaginatedList<T> : List<T>
    {
        public int CurrentPage { get; }
        public int ItemsPerPage { get; }
        public int StartPage { get; }
        public int EndPage { get; }
        public int TotalPages { get; }
        public int TotalItems { get; }
        public bool HasPreviousPage
        {
            get
            {
                return CurrentPage > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }

        public PaginatedList(List<T> items, int count, int page, int pageSize)
        {
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            int currentPage = page > 1 ? page : 1;
            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage -= startPage - 1;
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            CurrentPage = currentPage;
            ItemsPerPage = pageSize;
            TotalPages  = totalPages;
            TotalItems = count;
            StartPage   = startPage;
            EndPage     = endPage;

            AddRange(items);
        }


        public static PaginatedList<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            int count = source.Count();
            List<T> items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}