namespace SampleApp;

[Generators.Report]
public partial class SampleEntity
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int Age { get; init; }
}

// This code will not compile until you build the project with the Source Generators