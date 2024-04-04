using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientAppCore.Models
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public PaginatedResult(List<T> items, int pageNumber, int totalPages)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalPages = totalPages;
        }
    }
}
