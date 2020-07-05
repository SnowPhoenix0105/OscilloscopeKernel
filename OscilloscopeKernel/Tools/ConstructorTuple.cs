using System;
using System.Collections.Generic;
using System.Text;

namespace OscilloscopeKernel.Tools
{
    public class ConstructorTuple<T>
    {
        private Type type;
        private Object[] parameters;

        public ConstructorTuple(Type type, params Object[] parameters)
        {
            this.type = type;
            this.parameters = parameters;
        }

        public T NewInstance()
        {
            return (T)Activator.CreateInstance(type, parameters);
        } 
    }
}
