using System;
using System.Threading;

namespace ParallelMSpecRunner.Utils
{
    internal class InvokeOnce<T>
    {
        private readonly Action<T> _invocation;
        private int _wasInvoked;

        private const int TRUE = 1;
        private const int FALSE = 0;

        public InvokeOnce(Action<T> invocation)
        {
            _invocation = invocation;
            _wasInvoked = FALSE;
        }

        public void Invoke(T value)
        {
            if (Interlocked.CompareExchange(ref _wasInvoked, TRUE, FALSE) == FALSE)
                _invocation(value);
        }

        public bool WasInvoked {
            get { return _wasInvoked == TRUE; }
        }
    }
}