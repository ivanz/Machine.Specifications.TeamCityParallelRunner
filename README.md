## MSpec TeamCity Parallel Runner (mspec-teamcity-prunner.exe)

This runner is similar to the standard runner with a few key differences:

* Tests in multiple assemblies **are executed in parallel** (one thread per assembly). The default limit is 2 threads (so 2 assemblies in parallel at a time). This can be controlled by the `--threads` parameter.
* Assemblies to run are specified using the `--assembly` in a `,` separated list. 
	* **Note:** This is different from the standard console runner where the `--assembly` part is not required.
* Or - you can specifiy a directory to recursively search for .dll files through `--directory` 
	* Then you can use `--pattern` to apply a .NET Regex filter on the file list

**Note**: With this runner the Tests output (number of tests and results) will be delayed until the end of the test run and you won't see an incrementing number of tests. You will be able to navigate around the results as usual.

## Example Usage

The following configuration has been verified to work:
* TeamCity 8.1.x with a Command Line Runner

```
mspec-teamcity-prunner.exe -t 4 -d "%teamcity.build.workingDir%" -p "\\bin\\.*\.Tests.dll$"
```

or

```
mspec-teamcity-prunner.exe -t 4 -a Assembly1.dll,Assembly2.dll
```

## Installation

Right now the only method for installation is from source. Download the solution and build it and copy the binaries folder to your TeamCity instance if you want to take it for a spin. 

Depending on where does https://github.com/machine/machine.specifications/issues/252 take us - I will either integrate it into MSpec itself or keep it as a separate util and get a NuGet package published.

## Command Line Parameters

```

Machine.Specifications TeamCity Parallel Runner
Copyright (C) 2007-2014 Ivan Zlatev, Machine.Specifications Project (based heavily on the Machine.Specifications.ConsoleRunner)

mspec-teamcity-prunner --threads 4 --assembly Test1.dll,Test2.dll
Options:
  -a, --assembly              Specify an explicit comma-separated list of assemblies
  -d, --directory             Optionally use to specify a directory to recursively look for *.dll files
  -p, --pattern               Used in combination with -d provides a way to specify a patter (.Net regex) to search for - example: \\bin\\.*\.Tests.dll
  -f, --filters               Filter file specifying contexts to execute (full type name, one per line). Takes precedence over tags
  -i, --include               Execute all specifications in contexts with these comma delimited tags. Ex. -i "foo,bar,foo_bar"
  -x, --exclude               Exclude specifications in contexts with these comma delimited tags. Ex. -x "foo,bar,foo_bar"
  -t, --threads               Number of parallel threads.
  -h, --help                  Shows this help message
```
