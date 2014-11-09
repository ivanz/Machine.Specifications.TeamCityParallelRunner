using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using CommandLine;
using Machine.Specifications.Runner.Utility;

namespace ParallelMSpecRunner
{
    public class Options
    {
        public Options()
        {
            AssemblyFiles = null;
            ExcludeTags = null;
            IncludeTags = null;
            DisableTeamCityAutodetection = false;
            TeamCityIntegration = false;
            ShowTimeInformation = false;
            NoColor = false;
            Progress = false;
            Silent = false;
            FilterFile = string.Empty;
            HtmlPath = string.Empty;
            XmlPath = string.Empty;
            Threads = 2;
        }


        [Option("threads",
                HelpText = "Number of threads to use")]
        public int Threads { get; set; }

        [Option("xml", HelpText = "Outputs the XML report to the file referenced by the path")]
        public string XmlPath { get; set; }

        [Option("html",
          HelpText = "Outputs the HTML report to path, one-per-assembly w/ index.html (if directory, otherwise all are in one file)")]
        public string HtmlPath { get; set; }

        [Option('f',
          "filter",
          HelpText = "Filter file specifying contexts to execute (full type name, one per line). Takes precedence over tags")]
        public string FilterFile { get; set; }

        [Option('s',
          "silent",
          HelpText = "Suppress progress output (print fatal errors, failures and summary)")]
        public bool Silent { get; set; }

        [Option('p',
          "progress",
          HelpText = "Print dotted progress output")]
        public bool Progress { get; set; }

        [Option('c',
          "no-color",
          HelpText = "Suppress colored console output")]
        public bool NoColor { get; set; }

        [Option('t',
          "timeinfo",
          HelpText = "Adds time-related information in HTML output")]
        public bool ShowTimeInformation { get; set; }

        [Option("teamcity",
          HelpText = "Reporting for TeamCity CI integration (also auto-detected)")]
        public bool TeamCityIntegration { get; set; }

        [Option("no-teamcity-autodetect",
          HelpText = "Disables TeamCity autodetection")]
        public bool DisableTeamCityAutodetection { get; set; }

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

        [ValueList(typeof(List<string>))]
        public IList<string> AssemblyFiles { get; set; }

        [Option('w',
          "wait",
          HelpText = "Wait for debugger to be attached")]
        public bool WaitForDebugger { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Machine.Specifications TeamCity Parallel Runner (mspec-teamcity-prunner)");
            sb.AppendLine("Copyright (C) 2007-2014 Ivan Zlatev, Machine.Specifications Project (based on the Machine.Specifications.ConsoleRunner)");
            sb.AppendLine("");
            sb.AppendLine(Usage());
            sb.AppendLine("Options:");
            sb.AppendLine("  --threads                   Number of threads to use. Default is 2.");
            sb.AppendLine("  -f, --filters               Filter file specifying contexts to execute (full type name, one per line). Takes precedence over tags");
            sb.AppendLine("  -i, --include               Execute all specifications in contexts with these comma delimited tags. Ex. -i \"foo,bar,foo_bar\"");
            sb.AppendLine("  -x, --exclude               Exclude specifications in contexts with these comma delimited tags. Ex. -x \"foo,bar,foo_bar\"");
            sb.AppendLine("  -t, --timeinfo              [NOT SUPPORTED] Shows time-related information in HTML output");
            sb.AppendLine("  -s, --silent                [NOT SUPPORTED] Suppress progress output (print fatal errors, failures and summary)");
            sb.AppendLine("  -p, --progress              [NOT SUPPORTED] Print progress output");
            sb.AppendLine("  -c, --no-color              [NOT SUPPORTED] Suppress colored console output");
            sb.AppendLine("  -w, --wait                  [NOT SUPPORTED] Wait 15 seconds for debugger to be attached");
            sb.AppendLine("  --teamcity                  [ALWAYS ON] Reporting for TeamCity CI integration (also auto-detected)");
            sb.AppendLine("  --no-teamcity-autodetect    [DOES NOTHING] Disables TeamCity autodetection");
            sb.AppendLine("  --html <PATH>               [NOT SUPPORTED] Outputs the HTML report to path, one-per-assembly w/ index.html (if directory, otherwise all are in one file)");
            sb.AppendLine("  --xml <PATH>                [NOT SUPPORTED] Outputs the XML report to the file referenced by the path");
            sb.AppendLine("  -h, --help                  Shows this help message");

            return sb.ToString();
        }

        public static string Usage()
        {
            var runnerExe = Assembly.GetEntryAssembly();
            return String.Format("{0} --threads 4 <assemblies>", Path.GetFileName(runnerExe != null ? runnerExe.Location : "mspec-teamcity-prunner.exe"));
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

            return RunOptions.Custom
                .Include(IncludeTags ?? new string[0])
                .Exclude(ExcludeTags ?? new string[0])
                .FilterBy(filters);
        }
    }
}
