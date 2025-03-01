using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Common.Exeptions
{
    public class ValidationExeption : Exception
    {
        public List<string> Errors { get; private set; }
        public ValidationExeption(IEnumerable<string> errors) :base(string.Join("\n", errors))
        {
            Errors = new List<string>(errors);
        }
    }
}
