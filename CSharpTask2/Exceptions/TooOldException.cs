using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTask2.Exceptions
{
    class TooOldException : Exception
    {
        public TooOldException()
            : base("Date of birth is unrealistically far in the past") { }
    }
}
