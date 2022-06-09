using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using MediatR;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Conduit.FitnessFunctions.ArchitectureTests
{
    public class ArchitectureTestSetupFixture : IDisposable
    {
        public Architecture Architecture { get; }
        public IObjectProvider<Class> DomainContractClasses { get; private set; }
        public IObjectProvider<Class> DomainOperations { get; private set; }
        public IObjectProvider<Class> Commands { get; private set; }
        public IObjectProvider<Class> Queries { get; private set; }
        public IObjectProvider<Class> DatabaseMigrations { get; private set; }

        public ArchitectureTestSetupFixture()
        {
            Architecture = new ArchLoader().LoadAssemblies(GetSolutionAssemblies()).Build();
            SetupDomainContracts();
            SetupDatabaseMigrations();
        }

        private void SetupDatabaseMigrations()
        {
            DatabaseMigrations = Classes().That().ResideInNamespace(".*Domain.Migrations.*", true)
                .And().AreNot(".*ProcessedByFody", true)
                .As("Database Migrations");
        }
        
        private void SetupDomainContracts()
        {
            SetupDomainContractClasses();
            SetupOperations();
            SetupCommands();
            SetupQueries();
        }
        private void SetupDomainContractClasses()
        {
            DomainContractClasses = Classes().That().ResideInAssembly(".*Domain.Contracts.*", true)
                .And().AreNot(".*ProcessedByFody", true)
                .As("Domain Contracts");
        }

        private void SetupOperations()
        {
            DomainOperations = Classes().That().Are(DomainContractClasses)
                .And().ImplementInterface(typeof(IRequest<>))
                .As("Domain Operations");
        }
        
        private void SetupCommands()
        {
            Commands = Classes().That().Are(DomainOperations)
                .And().HaveNameEndingWith("Command")
                .As("Commands");
        }
        
        private void SetupQueries()
        {
            Queries = Classes().That().Are(DomainOperations)
                .And().HaveNameEndingWith("Query")
                .As("Queries");
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