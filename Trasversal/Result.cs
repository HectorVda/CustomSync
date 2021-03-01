using System;
using System.Collections.Generic;

namespace Trasversal
{
    public class Result
    {
        public bool ActionResult { get; set; } = true;
        public IList<string> Errors { get; set; }
        public string ErrorCode { get; set; }

        public Result(bool actionResult = true)
        {
            this.ActionResult = actionResult;
            this.Errors = new List<string>();
        }
    }
    public class Result<T> : Result
    {
        public T ResultObject { get; set; }

        public Result(bool actionResult = true) : base(actionResult)
        {
            
          
        }

        
    }
}
