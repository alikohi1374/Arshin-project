using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0_Framework.Application
{
    public class OperationResult
    {
        public bool IsSucceeded { get; set; }
        public string Message { get; set; }


        public OperationResult()
        {
            IsSucceeded = false;
        }
        public OperationResult Succeeded(string message = "عملیات با موفیقت انجام شد ")
        {
            IsSucceeded = true;
            Message = message;
            return this;
        }

        public OperationResult Failed(string message)
        {
            IsSucceeded = true;
           Message = message;
            return this;
        }

    }
}
