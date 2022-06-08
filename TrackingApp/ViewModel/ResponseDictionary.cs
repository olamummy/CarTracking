using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingApp.ViewModel
{
    public class ResponseDictionary
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string ExceptionMessage { get; set; }
        public Object data { get; set; }
    }
}
