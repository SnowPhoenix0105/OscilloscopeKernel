using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Exceptions
{
    class DifferentGraphSizeException : Exception
    {
        public DifferentGraphSizeException(): base() { }

        public DifferentGraphSizeException(string message): base(message) { }

        public DifferentGraphSizeException(string message, Exception inner) : base(message, inner) { }
    }
}
