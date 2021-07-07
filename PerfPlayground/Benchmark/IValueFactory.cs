using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Diagnostics.Runtime.Interop;

namespace PerfPlayground.Benchmark
{
    public interface IValueFactory
    {
        object GetValue();
    }

    internal class DoubleCheckedLockingFactory : IValueFactory
    {
        private volatile object _value;
        private readonly object synObj = new object();

        public object GetValue()
        {
            if (_value == null)
            {
                lock (synObj)
                {
                    if (_value == null)
                    {
                        _value = new object();
                    }
                }
            }

            return _value;
        }
    }

    internal class NaiveLockingFactory : IValueFactory
    {
        private volatile object _value;
        private readonly object synObj = new object();

        public object GetValue()
        {
            lock (synObj)
            {
                if (_value == null)
                {
                    _value = new object();
                }
            }
            return _value;
        }
    }

    internal class LazyLockingFactory : IValueFactory
    {
        private readonly Lazy<object> _value = new Lazy<object>(() => new object());

        public object GetValue()
        {
            return _value.Value;
        }
    }


}
