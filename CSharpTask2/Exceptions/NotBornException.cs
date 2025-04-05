using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTask2.Exceptions
{
    class NotBornException : Exception
    {
        public NotBornException()
            : base("Date of birth cannot be in the future") { }
    }
}
