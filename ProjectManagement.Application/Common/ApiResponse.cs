using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }

        public List<string>? ErrorMessages { get; set; }
        public T? Data { get; set; }

        public int StatusCode { get; set; }

       
    }
}
