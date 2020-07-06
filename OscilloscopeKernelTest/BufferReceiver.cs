using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernelTest
{
    //TODO
    public class BufferReceiver<T>
    {
        private ConcurrentQueue<T> buffer;

        public BufferReceiver(ConcurrentQueue<T> buffer)
        {
            this.buffer = buffer;
        }
    }
}
