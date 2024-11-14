# Playing with Roslyn Source Generator

- This repository contains 3 projects as a playground for Source Generator
- Class library, the Source Generator itself
- Console application for manual testing
- Unit Test project that uses `CSharpGeneratorDriver` to run tests against the generated results

## Resources

- [Incremental source generator with Roslyn](https://youtu.be/BfYxZ4mfv0E) 📽️*1hour - Stefan Pölz - NDC Oslo 2023*
- [Quoter](https://roslynquoter.azurewebsites.net) 📓*Roslyn compiler platform - C# code to syntax tree*
- List of [C# Source Generators](https://github.com/amis92/csharp-source-generators) 👤
  - [AutoEntityBuilder](https://github.com/mhdbouk/AutoBuilder) 👤Mohamad Dbouk
  - [Minimal APIs Helpers](https://github.com/marcominerva/MinimalHelpers) 👤Marco Minerva
- Series: [Creating an incremental generator](https://andrewlock.net/series/creating-a-source-generator) 📓*Andrew Lock*
- [Source Generators](https://code-maze.com/csharp-source-generators) 📓*Code-Maze*
- [Debug Source Generators in Rider](https://blog.jetbrains.com/dotnet/2023/07/13/debug-source-generators-in-jetbrains-rider) 📓*JetBrains Blog*

## Note

To get the **Syntax Visualizer** view in Visual Studio, select the following options during the installation

- Other Toolsets / Visual Studio extension development *(.NET Compiler Platform SDK)*
- Individual components: DGML editor
- View / Other Windows / Syntax Visualizer

