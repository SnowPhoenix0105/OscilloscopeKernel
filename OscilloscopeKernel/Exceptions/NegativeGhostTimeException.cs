using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Exceptions
{
    public class NegativeGhostTimeException : ArgumentException
    {
        public NegativeGhostTimeException() : base() { }

        public NegativeGhostTimeException(string msg) : base(msg) { }

        public NegativeGhostTimeException(string msg, Exception inner) : base(msg, inner) { }
    }
}
