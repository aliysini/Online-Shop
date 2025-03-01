using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Common.Exeptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException() : base("نام کاربری وارد شده قبلاً ثبت شده است.")
        {
        }
    }
}
