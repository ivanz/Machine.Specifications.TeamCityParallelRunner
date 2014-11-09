using System;
using System.Text;
using Machine.Specifications.Reporting.Integration;
using Machine.Specifications.Reporting.Integration.TeamCity;
using Machine.Specifications.Runner.Utility;
using ParallelMSpecRunner.Utils;

namespace ParallelMSpecRunner.Reporting
{
    /// <summary>
    /// The idea here is that we buffer all output and then once we are done - 
    /// we signal the owner.
    /// </summary>
    public class BufferedAssemblyTeamCityReporter : ISpecificationRunListener, ISpecificationResultProvider
    {
        private readonly TeamCityReporter _reporter;
        private readonly StringBuilder _buffer;
        private readonly InvokeOnce<BufferedAssemblyTeamCityReporter> _onFinished;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="onFinished">Gets invoked when the runner is done</param>
        public BufferedAssemblyTeamCityReporter(Action<BufferedAssemblyTeamCityReporter> onFinished)
        {
            _buffer = new StringBuilder();
            _onFinished = new InvokeOnce<BufferedAssemblyTeamCityReporter>(onFinished);
            _reporter = new TeamCityReporter(WriteToBuffer, new TimingRunListener());
        }

        private void WriteToBuffer(string text)
        {
            _buffer.AppendLine(text);
        }

        public string Buffer {
            get { return _buffer.ToString(); }
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