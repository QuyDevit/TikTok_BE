using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiktokBackend.Application.Common
{
    public class PagedResponse<T> : ServiceResponse<List<T>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
        public PagedResponse(List<T> data, int pageNumber, int pageSize, int totalRecords, string message = "")
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            Success = true;
            Message = message;
        }
        public static PagedResponse<T> Create(List<T> data, int pageNumber, int pageSize, int totalRecords, string message = "")
        {
            return new PagedResponse<T>(data, pageNumber, pageSize, totalRecords, message);
        }
    }
}
