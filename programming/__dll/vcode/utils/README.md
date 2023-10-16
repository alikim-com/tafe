[Utils assembly][def]

> How to get files:

1\. Compile your own assembly from the source files:

- Download all files from ``utils`` folder, open the folder in VS Code or Visual Studio and compile the project/solution.

2\. Use compiled assembly files:

- Download all files from ``utils/bin`` folder

> How To Use Files in your projects:

- In VS Code, add ``ItemGroup`` section to your ``.csproj`` file, use one of two options
```
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net7.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

   <ItemGroup>
      <!-- EITHER option 1: reference external project -->
      <ProjectReference Include="(absolute path to the folder)\utils\utils.csproj" />
      <!-- OR option 2: reference an external dll file -->
      <Reference Include="(path to the dll file)\dll\utils.dll" />
   </ItemGroup>

</Project>
```
- in your ``.cs`` files

```
using static utils.UserInput;
using static utils.Output;

class Program
{
   public static void Main(string[] args)
   {
      // console output shortcut
      cwl("Hello, world!");

      // get sanitized user input: an int number within the specified range
      string prompt = "Please enter your score (1-100) or 'Enter' to exit: ";
      if (UntilSafeCustom<int>(IntRange, new int[] { 1, 100 }, out int score, prompt, exit: "") != 0) return;
   }
}

```

[def]: https://github.com/alikim-com/tafe/blob/main/programming/__dll/vcode/utils/