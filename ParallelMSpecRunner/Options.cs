using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using CommandLine;
using Machine.Specifications.Runner;

namespace ParallelMSpecRunner
{
    public class Options
    {
        public Options()
        {
            AssemblyFiles = null;
            ExcludeTags = null;
            IncludeTags = null;
            FilterFile = string.Empty;
            Threads = 2;
        }

        [Option('d',
        "directory",
        HelpText = "Look recursively for test assemblies in this folder")]
        public string TestsDirectory { get; set; }

        [OptionList('p',
        "pattern",
        HelpText = "File pattern to look for (e.g,: *.Tests.dll)",
        Separator = ',')]
        public IList<string> TestsFilePatterns { get; set; }

        [Option('f',
        "filter",
        HelpText = "Filter file specifying contexts to execute (full type name, one per line). Takes precedence over tags")]
        public string FilterFile { get; set; }

        [Option('t',
        "threads",
        HelpText = "Number of parallel processes")]
        public uint Threads { get; set; }

        [OptionList('i',
        "include",
        HelpText = "Execute all specifications in contexts with these comma delimited tags. Ex. -i \"foo,bar,foo_bar\"",
        Separator = ',')]
        public IList<string> IncludeTags { get; set; }

        [OptionList('x',
        "exclude",
        HelpText = "Exclude specifications in contexts with these comma delimited tags. Ex. -x \"foo,bar,foo_bar\"",
        Separator = ',')]
        public IList<string> ExcludeTags { get; set; }

        [OptionList('a',
        "assembly",
        HelpText = "Comman separated list of assemblies",
        Separator = ',')]
        public IList<string> AssemblyFiles { get; set; }

        [Option('w',
        "wait",
        HelpText = "Wait for debugger to be attached")]
        public bool WaitForDebugger { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Machine.Specifications TeamCity Parallel Runner");
            sb.AppendLine("Copyright (C) 2007-2014 Ivan Zlatev, Machine.Specifications Project (based heavily on the Machine.Specifications.ConsoleRunner)");
            sb.AppendLine("");
            sb.AppendLine(Usage());
            sb.AppendLine("Options:");
            sb.AppendLine("  -t, --threads               Number of parallel threads.");
            sb.AppendLine("  -a, --assembly              Specify an explicit comma-separated list of assemblies");
            sb.AppendLine("  -d, --directory             Optionally use to specify a directory to recursively look for *.dll files");
            sb.AppendLine("  -p, --pattern               Used in combination with -d provides a way to specify a patter (.Net regex) to search for - example: \\\\bin\\\\.*\\.Tests.dll$");
            sb.AppendLine("  -f, --filters               Filter file specifying contexts to execute (full type name, one per line). Takes precedence over tags");
            sb.AppendLine("  -i, --include               Execute all specifications in contexts with these comma delimited tags. Ex. -i \"foo,bar,foo_bar\"");
            sb.AppendLine("  -x, --exclude               Exclude specifications in contexts with these comma delimited tags. Ex. -x \"foo,bar,foo_bar\"");
            sb.AppendLine("  -h, --help                  Shows this help message");

            return sb.ToString();
        }

        public static string Usage()
        {
            return String.Format("{0} --threads 4 --assembly Test1.dll,Test2.dll", Process.GetCurrentProcess().ProcessName);
        }

        public virtual bool ParseArguments(string[] args)
        {
            return Parser.Default.ParseArguments(args, this);
        }

        public virtual RunOptions GetRunOptions()
        {
            var filters = new string[0];
            if (!String.IsNullOrEmpty(FilterFile)) {
                filters = File.ReadAllLines(FilterFile, Encoding.UTF8);
            }

            return new RunOptions(IncludeTags ?? new string[0], ExcludeTags ?? new string[0], filters);
        }
    }
}
