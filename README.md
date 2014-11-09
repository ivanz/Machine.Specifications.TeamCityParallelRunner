## MSpec TeamCity Parallel Runner (mspec-teamcity-prunner.exe)

**`mspec-teamcity-prunner.exe`** is a **drop-in replacement** for the standard mspec runner which runs test assemblies **in parallel**. The NuGet package is called 'Machine.Specifications.TeamCityParallelRunner'.

Use the `--threads N` parameter to specify how many assemblies to process at the same time.

Some differences with the standard runner:

* Some command line options are not supported and won't do anything if used. Details below.
* There is no real-time progress monitoring ("10 tests ran", "15 test ran", etc.), but full results are available the same way as in the standard runner.


Note that some command line options are not supported and won't do anything if specified and have been kept only for compatibility. See below for details.

## Installation

### NuGet Package

The NuGet Package is here: https://www.nuget.org/packages/Machine.Specifications.TeamCityParallelRunner

Once installed replace the path to `mspec.exe` in your `MSpec Build Step` with `tools\mspec-teamcity-runner.exe` from the package.

### From Source

Download, compile, copy `mspec-teamcity-runner.exe` to your TeamCity instance and replace the path to `mspec.exe` with the path to `mspec-teamcity-runner.exe` in a `MSpec Build Step`.

## Command Line Parameters

```

Machine.Specifications TeamCity Parallel Runner (mspec-teamcity-prunner)
Copyright (C) 2007-2014 Ivan Zlatev, Machine.Specifications Project (based on the Machine.Specifications.ConsoleRunner)

mspec-teamcity-prunner.exe --thread 4 <assemblies>
Options:
  --threads                   Number of threads to use. Default is 2.
  -f, --filters               Filter file specifying contexts to execute (full type name, one per line). Takes precedence over tags
  -i, --include               Execute all specifications in contexts with these comma delimited tags. Ex. -i "foo,bar,foo_bar"
  -x, --exclude               Exclude specifications in contexts with these comma delimited tags. Ex. -x "foo,bar,foo_bar"
  -t, --timeinfo              [NOT SUPPORTED] Shows time-related information in HTML output
  -s, --silent                [NOT SUPPORTED] Suppress progress output (print fatal errors, failures and summary)
  -p, --progress              [NOT SUPPORTED] Print progress output
  -c, --no-color              [NOT SUPPORTED] Suppress colored console output
  -w, --wait                  [NOT SUPPORTED] Wait 15 seconds for debugger to be attached
  --teamcity                  [ALWAYS ON] Reporting for TeamCity CI integration (also auto-detected)
  --no-teamcity-autodetect    [DOES NOTHING] Disables TeamCity autodetection
  --html <PATH>               [NOT SUPPORTED] Outputs the HTML report to path, one-per-assembly w/ index.html (if directory, otherwise all are in one file)
  --xml <PATH>                [NOT SUPPORTED] Outputs the XML report to the file referenced by the path
  -h, --help                  Shows this help message
mspec-teamcity-prunner.exe --thread 4 <assemblies>
```
