using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTask2.Exceptions
{
    class NoWesternSignException : Exception
    {
        public NoWesternSignException()
            : base("For given month is no valid western sign") { }
    }
}
