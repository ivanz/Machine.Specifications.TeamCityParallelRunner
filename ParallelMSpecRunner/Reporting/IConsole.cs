namespace ParallelMSpecRunner.Reporting
{
    public interface IBufferedConsole
    {
        void Write(string line);
        void WriteLine(string line);
        string GetBuffer();
    }
}
