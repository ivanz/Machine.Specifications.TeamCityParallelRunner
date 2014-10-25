## MSpec TeamCity Parallel Runner (mspec-teamcity-prunner.exe)

This runner is similar to the standard runner with a few key differences:

* Tests in multiple assemblies **are executed in parallel** (one thread per assembly). The default limit is 2 threads (so 2 assemblies in parallel). This can be controlled by the `--threads` parameter.
* Assemblies to run are specified using the `--assembly` in a `,` separated list. **Note:** This is different from the standard console runner where the `--assembly` part is not required.
* Or - you can specifiy a directory to recursively search for .dll files through `--directory` 
	* Then you can use `--pattern` to apply a .NET Regex filter on the file list

***Example Usage***

The following configuration has been verified to work:
* TeamCity 8.1.x with a Command Line Runner

```
mspec-teamcity-prunner.exe -t 4 -d "%teamcity.build.workingDir%" -p "\\bin\\.*\.Tests.dll$"
```

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
