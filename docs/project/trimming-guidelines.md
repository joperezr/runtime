Linker Annotations and Trimming tests Guidelines
================================================

This document assumes previous familiarity with the [Linking .NET Libraries design doc](https://github.com/dotnet/designs/blob/master/accepted/2020/linking-libraries.md) and will not go into details of each concept already explained there.

# Overview #

The high level overview is that for .NET 5, there will be an effort to make the .NET Libraries work better with the [Linker](https://github.com/mono/linker) so that when size is a concern, it will be able to trim out unused code and only keep that which each individual app requires to run succesfully. There are some cases where the Linker can't accurately know which code is being used by a method (for example if when it is calling methods via reflection or using serialization features). For cases like this, these methods may add annotations to let the Linker know which dynamic dependencies they might have so that the Linker knows to preserve those dynamic dependencies as well.

# Testing #

In order to test that the annotations added to the methods are correct, we need to ensure that when they are called, the Linker must preserve those dynamic dependencies so that the methods work correctly. For that we have built the Trimming Test infrastructure.

# Trimming Test Infrastructure #

It consists basically on individual `.cs` files (each representing one test case), which will execute some code causing some dynamic dependency to be called and should not get trimmed. The infrastructure will grab each test case (each `.cs` file) and generate a console application with it, publish it, trim it using the Linker, and execute it. Finally the infrastructure will check to make sure that the return code matches a success code (application should return a 100 exit code indicating success) and if it got a different code instead then it will assume error was caused due to the Linker trimming out a dynamic dependency and will error and fail the test.

# Adding Trimming Tests #

Trimming Tests are usually located under `src\<Library Name>\tests\TrimmingTests\`. That folder will mainly contain two things: A Test project called `<Library-Name>.TrimmingTest.proj` and individual `.cs` files each representing a test case.

The `TrimmingTest.proj` file should ONLY contain:
```xml
<Project DefaultTargets="Build">
  <Import Project="$([MSBuild]::GetPathOfFileAbove(Directory.Build.props))" />

  <Import Project="$([MSBuild]::GetPathOfFileAbove(Directory.Build.targets))" />
</Project>
```
and its only purpose is so that we can invoke the test infrastructure so we gather all tests on this directory and execute them. Note that the `.cs` files (or test cases) will NOT be added into this project as `Compile` items, since this project won't really be compiling them.

Each test case will consist of one `.cs` file that will be the default basic console app boiler plate code, plus some code which will cause the annotated method to get called and to call the dynamic dependencies it requires. Finally, the test case should return a 100 exit code which will mean that if all ran to completion and reached that return statement, the test should be considered as passed. Here is an example of what a test case could look like:
```c#
using System;

class Program
{
    static int Main(string[] args)
    {
        // Foo.Bar() implementation uses reflection to call some dynamic dependency
        // so if it has been annotated correctly, then the linker should keep that
        // dynamic dependency as well and the call to Foo.Bar() should succeed and 
        // the app will finish executing and return a 100 exit code which the Trimming
        // infrastructure will interpret as test passed.
        Foo.Bar();
        // If Foo.Bar() wasn't annotated correctly, then it will most likely throw some
        // internal exception (like a MissingMemberException or TypeNotFoundException)
        // which will cause the console app to return a non-100 exit code which the Trimming
        // infrastructure will interpret as a test failed.
        return 100;
    }
}
```
Few notes about this. Note that we are really trying to make these console apps as minimal as possible, and that is because we want to avoid some extra code rooting more types/members which may cause the test to pass even when it hasn't been annotated correctly. For the same reason, we decided to validate test passing using only a return code, as opposed to using frameworks like `Assert` as those would most likely end up rooting more code than desired. Finally, there will be some members that  aren't testable like this given that the annotated member is not public (and  can't be easily called by any other public method), or not callable (like some member on an attribute type), so this infrastructure won't work for such cases.

# Running Trimming Tests #

Trimming tests use the live-built runtime, so for that reason, they have as prerequirement that you must have first ran a full clr and libs build by running: `build.cmd -s clr+libs [optional other args]` in Windows or `build.sh -s clr+libs [optional other args]` in Unix.

Once you have finished authoring your tests and built clr and libs partitions, there are several ways to run them:

- Run them from the root of the repo by calling `build.cmd -s libs.tests /p:TestAssemblies=false /p:TestTrimming=true [optional other args]` in Windows or `build.sh -s libs.tests /p:TestAssemblies=false /p:TestTrimming=true [optional other args]` in Unix.
- Run them from the individual TrimmingTest project by navigating to `src/<Library Name>/tests/TrimmingTests/` and running:
    - `dotnet build`
    - `dotnet test`
    - `dotnet msbuild`

To see the generated projects and the Trimmed output, navigate to `<Repo Root>\artifacts\bin\trimmingTests\projects\<Library Name>.TrimmingTests\<Test Case Name>\<architecture>` which will contain the `project.csproj` file along with the `*.cs` file with your test, and a `bin` folder where the published app is located.