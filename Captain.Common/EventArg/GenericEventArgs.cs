using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Captain.Common.EventArg
{
    public class GenericEventArgs<T> : EventArgs
    {
        public GenericEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}
