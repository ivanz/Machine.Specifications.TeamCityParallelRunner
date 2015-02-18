using System.Text;

namespace ParallelMSpecRunner.Reporting
{
    public class DefaultBufferedConsole : IBufferedConsole
    {
        private readonly StringBuilder _buffer = new StringBuilder();

        public void Write(string line)
        {
            _buffer.Append(line);
        }

        public void WriteLine(string line)
        {
            _buffer.AppendLine(line);
        }
        
        public string GetBuffer()
        {
            return _buffer.ToString();
        }
    }
}