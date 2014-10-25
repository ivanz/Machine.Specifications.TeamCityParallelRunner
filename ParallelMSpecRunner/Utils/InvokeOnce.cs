using System;

namespace ParallelMSpecRunner.Utils
{
    internal class InvokeOnce<T>
    {
        private readonly Action<T> _invocation;

        public InvokeOnce(Action<T> invocation)
        {
            _invocation = invocation;
        }

        public void Invoke(T value)
        {
            if (WasInvoked)
                return;

            WasInvoked = true;
            _invocation(value);
        }

        public bool WasInvoked { get; private set; }
    }
}