using System;
using System.Reflection;
using System.Runtime.InteropServices;

Console.WriteLine("=== .NET Version Test for GitHub Copilot ===");
Console.WriteLine();

// Display environment information
Console.WriteLine($"Environment Version: {Environment.Version}");
Console.WriteLine($"Framework Description: {RuntimeInformation.FrameworkDescription}");
Console.WriteLine($"Runtime Identifier: {RuntimeInformation.RuntimeIdentifier}");
Console.WriteLine($"OS Description: {RuntimeInformation.OSDescription}");
Console.WriteLine($"OS Architecture: {RuntimeInformation.OSArchitecture}");
Console.WriteLine($"Process Architecture: {RuntimeInformation.ProcessArchitecture}");
Console.WriteLine();

// Display assembly information
var assembly = Assembly.GetExecutingAssembly();
Console.WriteLine($"Assembly Location: {assembly.Location}");
Console.WriteLine($"Assembly Framework: {assembly.GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>()?.FrameworkName}");
Console.WriteLine();

// Test some .NET features
Console.WriteLine("=== Feature Tests ===");

// Test string interpolation (available in all versions)
var name = "Copilot";
Console.WriteLine($"String interpolation test: Hello, {name}!");

// Test pattern matching (C# 7.0+)
object value = 42;
var result = value switch
{
    int i when i > 0 => "Positive integer",
    int i when i < 0 => "Negative integer", 
    int => "Zero",
    _ => "Not an integer"
};
Console.WriteLine($"Pattern matching test: {result}");

// Test nullable reference types (C# 8.0+)
string? nullableString = null;
Console.WriteLine($"Nullable reference type test: {nullableString ?? "null value handled"}");

// Test init-only properties (C# 9.0+)
try
{
    var record = new TestRecord { Name = "Test", Value = 123 };
    Console.WriteLine($"Init-only properties test: {record.Name} = {record.Value}");
}
catch (Exception ex)
{
    Console.WriteLine($"Init-only properties test failed: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("=== Summary ===");
Console.WriteLine("This test demonstrates the .NET capabilities available to GitHub Copilot.");
Console.WriteLine("The project is built with .NET 8.0 target framework.");

public record TestRecord
{
    public string Name { get; init; } = string.Empty;
    public int Value { get; init; }
}