using System;
using System.Collections.Generic;
using System.Text;

namespace com.tweetapp.application.Response
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
