# .NET Version Test Project

This project tests which version of .NET is available to GitHub Copilot.

## Files

- **DotNetVersionTest.csproj**: .NET 8.0 test project that builds successfully
- **DotNetVersionTest.Net9.csproj**: .NET 9.0 test project that fails to build
- **Program.cs**: Test application that displays environment information and tests C# features
- **run_dotnet_version_test.sh**: Automated test script that runs all tests
- **DOTNET_VERSION_REPORT.md**: Comprehensive report of findings

## Usage

### Run the automated test:
```bash
./run_dotnet_version_test.sh
```

### Run the .NET 8.0 test application:
```bash
dotnet run --project DotNetVersionTest.csproj
```

### Try building .NET 9.0 (will fail):
```bash
dotnet build DotNetVersionTest.Net9.csproj
```

## Key Findings

- ✅ Copilot supports .NET 8.0 (SDK 8.0.119, Runtime 8.0.19)
- ❌ Copilot does not support .NET 9.0
- ❌ Cannot build the main repository (requires .NET 9.0.305)
- ✅ Modern C# features (C# 9+) work with .NET 8.0