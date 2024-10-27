namespace SampleApp;

public static class Program
{
    public static void Main(string[] args)
    {
        var sampleEntity = new SampleEntity { Id = 1, Name = "Balazs", Age = 100 };

        Console.WriteLine(string.Join(", ", sampleEntity.Report())); // Report method is source generated
    }
}