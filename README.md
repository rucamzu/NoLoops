# No loops

## Motivation

I dislike loops. Loops encourage imperative code, which I dislike even more so. Whenever I can, which is pretty much always but in the more performance-sensitive places, I very much rather using [LINQ][microsoft-linq] expressions instead. Still, LINQ has its limitatons.

## Overview

*NoLoops* is a collection of extension methods for the [`IEnumerable<T>`][microsoft-ienumerable] and [`IAsyncEnumerable<T>`][microsoft-iasyncenumerable] interfaces, that I needed at one point and found  LINQ lacked.

## Using *NoLoops*

*NoLoops* is available as a [NuGet package][noloops-nuget] that you can install via your favourite package manager.

*NoLoops* complete source code is contained in a single source file, which means that you can alternatively pull or copy [NoLoops.cs][noloops-source] to your project.

## Documentation

*NoLoops* documentation is available at https://rucamzu.github.io/NoLoops/.


[noloops-nuget]: https://www.nuget.org/packages/NoLoops/
[noloops-source]: https://github.com/rucamzu/NoLoops/blob/main/src/NoLoops/NoLoops.cs

[microsoft-linq]: https://learn.microsoft.com/en-us/dotnet/api/system.linq
[microsoft-ienumerable]: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1
[microsoft-iasyncenumerable]: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1