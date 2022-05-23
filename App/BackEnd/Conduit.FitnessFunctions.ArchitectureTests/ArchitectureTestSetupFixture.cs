using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using JetBrains.Annotations;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Conduit.FitnessFunctions.ArchitectureTests
{
    public class ArchitectureTestSetupFixture : IDisposable
    {
        public Architecture Architecture { get; }
        public IObjectProvider<Class> DomainContractClasses { get; } = Classes().That().ResideInAssembly(".*Domain.Contracts.*", true)
                .And().AreNot(".*ProcessedByFody", true)
                .As("Domain Contracts");

        public ArchitectureTestSetupFixture()
        {
            Architecture = new ArchLoader().LoadAssemblies(GetSolutionAssemblies()).Build();
        }
        
        private static System.Reflection.Assembly[] GetSolutionAssemblies()
        {
            var projectNamespacePrefix = System.Reflection.Assembly.GetExecutingAssembly().FullName.Split(".")[0];
            var dlls = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            
            return dlls
                .Select(fileName => AssemblyName.GetAssemblyName(fileName))
                .Where(assemblyName => 
                    assemblyName.Name.StartsWith(projectNamespacePrefix) &&
                    !assemblyName.Name.Contains("Test"))
                .Select(assemblyName => System.Reflection.Assembly.Load(assemblyName))
                .ToArray();
        }
        
        public void Dispose()
        {
        }
    }
}