using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Microsoft.ComponentModel.Composition.Scripting.Util
{
    class DelayedInit<T>
    {
        T _value;
        readonly Func<T> _initializer;
        volatile bool _initialized;
        readonly ReaderWriterLockSlim _rwl = new ReaderWriterLockSlim();

        public DelayedInit(Func<T> initializer)
        {
            if (initializer == null)
                throw new ArgumentNullException("initializer");

            _initializer = initializer;
        }

        public T Value
        {
            get
            {
                try
                {
                    _rwl.EnterReadLock();
                    if (_initialized)
                        return _value;
                }
                finally
                {
                    if (_rwl.IsReadLockHeld)
                        _rwl.ExitReadLock();
                }

                try
                {
                    _rwl.EnterWriteLock();
                    if (!_initialized)
                    {
                        _value = _initializer();
                        _initialized = true;
                    }
                    return _value;
                }
                finally
                {
                    if (_rwl.IsWriteLockHeld)
                        _rwl.ExitWriteLock();
                }
            }
        }
    }
}
