using Spectre.Console.Testing;

namespace StepLang.CLI.Tests;

public class TypeRegistrarTest
{
    [Fact]
    public void TestTypeRegistrar()
    {
        new TypeRegistrarBaseTests(() => new ServiceCollectionTypeRegistrar()).RunAllTests();
    }
}