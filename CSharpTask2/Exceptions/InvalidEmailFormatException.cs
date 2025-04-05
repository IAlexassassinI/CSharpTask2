using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTask2.Exceptions
{
    public class InvalidEmailFormatException : Exception
    {
        public InvalidEmailFormatException()
            : base("Email format is invalid") { }
    }
}
