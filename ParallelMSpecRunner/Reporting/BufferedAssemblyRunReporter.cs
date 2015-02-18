using System;
using Machine.Specifications.Reporting.Integration;
using Machine.Specifications.Runner.Utility;
using ParallelMSpecRunner.Outputs;
using ParallelMSpecRunner.Utils;

namespace ParallelMSpecRunner.Reporting
{
    /// <summary>
    /// The idea here is that we buffer all output and then once we are done - 
    /// we signal the owner.
    /// </summary>
    public class BufferedAssemblyRunReporter : ISpecificationRunListener, ISpecificationResultProvider, IBuffer
    {
        private readonly RunListener _reporter;
        private readonly IBufferedConsole _console;
        private readonly InvokeOnce<BufferedAssemblyRunReporter> _onFinished;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onFinished">Gets invoked when the runner is done</param>
        /// <param name="options">Parameters from command line</param>
        public BufferedAssemblyRunReporter(Action<BufferedAssemblyRunReporter> onFinished, Options options)
        {
            _console = new DefaultBufferedConsole();
            var output = DetermineOutput(options, _console);
            _onFinished = new InvokeOnce<BufferedAssemblyRunReporter>(onFinished);
            _reporter = new RunListener(_console, output, new TimingRunListener());
        }

        public string Buffer
        {
            get { return _console.GetBuffer(); }
        }

        private IOutput DetermineOutput(Options options, IBufferedConsole console)
        {
            if (options.Silent)
            {
                return new SilentOutput();
            }

            return new VerboseOutput(console);
        }

        #region ISpecificationRunListener, ISpecificationResultProvider

        public bool FailureOccurred {
            get { return _reporter.FailureOccurred; }
        }

        public void OnAssemblyStart(AssemblyInfo assembly)
        {
            _reporter.OnAssemblyStart(assembly);
        }

        public void OnAssemblyEnd(AssemblyInfo assembly)
        {
            _reporter.OnAssemblyEnd(assembly);
        }

        public void OnRunStart()
        {
        }

        public void OnRunEnd()
        {
            _reporter.OnRunEnd();
            _onFinished.Invoke(this);
        }

        public void OnContextStart(ContextInfo context)
        {
            _reporter.OnContextStart(context);
        }

        public void OnContextEnd(ContextInfo context)
        {
            _reporter.OnContextEnd(context);
        }

        public void OnSpecificationStart(SpecificationInfo specification)
        {
            _reporter.OnSpecificationStart(specification);
        }


        public void OnSpecificationEnd(SpecificationInfo specification, Result result)
        {
            _reporter.OnSpecificationEnd(specification, result);
        }

        public void OnFatalError(ExceptionResult exception)
        {
            _reporter.OnFatalError(exception);
            _onFinished.Invoke(this);
        }

        #endregion

    }
}