using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Exceptions
{
    public class OscillocopeBuildException : Exception
    {
        public OscillocopeBuildException() : base() { }

        public OscillocopeBuildException(string msg) : base(msg) { }

        public OscillocopeBuildException(string msg, Exception inner) : base(msg, inner) { }
    }
}
