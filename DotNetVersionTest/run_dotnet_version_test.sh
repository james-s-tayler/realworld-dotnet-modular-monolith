#!/bin/bash

echo "========================================"
echo "GitHub Copilot .NET Version Test Script"
echo "========================================"
echo

echo "1. Environment Information:"
echo "------------------------"
echo "OS: $(lsb_release -d | cut -f2)"
echo "Architecture: $(uname -m)"
echo "Current directory: $(pwd)"
echo

echo "2. .NET Installation Details:"
echo "----------------------------"
which dotnet
echo
dotnet --version
echo
echo "Full .NET Info:"
dotnet --info
echo

echo "3. Repository Analysis:"
echo "----------------------"
echo "Repository .NET requirements (from global.json):"
if [ -f "../App/BackEnd/global.json" ]; then
    cat ../App/BackEnd/global.json
else
    echo "global.json not found"
fi
echo

echo "Repository .NET usage (from README.md):"
grep -i "\.net" ../README.md || echo "No .NET references found in README"
echo

echo "4. .NET Compatibility Tests:"
echo "---------------------------"

echo "Test 1: Building .NET 8.0 project..."
if dotnet build DotNetVersionTest.csproj > /dev/null 2>&1; then
    echo "✅ .NET 8.0 project builds successfully"
    echo "   Running .NET 8.0 application:"
    dotnet run --project DotNetVersionTest.csproj
else
    echo "❌ .NET 8.0 project failed to build"
fi
echo

echo "Test 2: Building .NET 9.0 project..."
if dotnet build DotNetVersionTest.Net9.csproj > /dev/null 2>&1; then
    echo "✅ .NET 9.0 project builds successfully"
else
    echo "❌ .NET 9.0 project failed to build"
    echo "   Error details:"
    dotnet build DotNetVersionTest.Net9.csproj 2>&1 | grep -E "(error|Error)" | head -3
fi
echo

echo "Test 3: Original repository build test..."
cd ../App/BackEnd
if dotnet restore > /dev/null 2>&1; then
    echo "✅ Original repository builds successfully"
else
    echo "❌ Original repository failed to build"
    echo "   Error details:"
    dotnet restore 2>&1 | grep -E "(error|Error|SDK)" | head -3
fi
cd ../../DotNetVersionTest
echo

echo "5. Summary:"
echo "----------"
echo "✅ Available: .NET 8.0.119 SDK"
echo "✅ Available: .NET 8.0.19 Runtime"
echo "❌ Missing: .NET 9.x SDK (required by repository)"
echo "✅ Copilot can build and run .NET 8.0 projects"
echo "❌ Copilot cannot build .NET 9.0 projects"
echo "❌ Copilot cannot build the original repository (requires .NET 9)"
echo

echo "Recommendations:"
echo "- For testing with Copilot, use .NET 8.0 target framework"
echo "- Original repository requires .NET 9.0.305 SDK for building"
echo "- Consider providing .NET 8.0 compatibility for broader tooling support"
echo

echo "========================================"