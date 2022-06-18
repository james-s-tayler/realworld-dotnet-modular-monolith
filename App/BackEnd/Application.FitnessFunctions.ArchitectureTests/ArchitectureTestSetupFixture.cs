using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using MediatR;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace Application.FitnessFunctions.ArchitectureTests
{
    public class ArchitectureTestSetupFixture : IDisposable
    {
        public Architecture Architecture { get; }
        public IObjectProvider<Class> DomainContractClasses { get; private set; }
        public IObjectProvider<Class> DomainOperations { get; private set; }
        public IObjectProvider<Class> DomainOperationHandlers { get; private set; }
        public IObjectProvider<Class> DomainClasses { get; private set; }
        public IObjectProvider<Class> Commands { get; private set; }
        public IObjectProvider<Class> CommandHandlers { get; private set; }
        public IObjectProvider<Class> Queries { get; private set; }
        public IObjectProvider<Class> QueryHandlers { get; private set; }
        public IObjectProvider<Class> DatabaseMigrations { get; private set; }

        public ArchitectureTestSetupFixture()
        {
            Architecture = new ArchLoader().LoadAssemblies(GetSolutionAssemblies()).Build();
            SetupDomainContracts();
            SetupDatabaseMigrations();
            SetupDomain();
        }

        private void SetupDatabaseMigrations()
        {
            DatabaseMigrations = Classes().That().ResideInNamespace(".*Domain.Migrations.*", true)
                .And().AreNot(".*ProcessedByFody", true)
                .As("Database Migrations");
        }

        private void SetupDomain()
        {
            SetupDomainClasses();
            SetupOperationHandlers();
            SetupCommandHandlers();
            SetupQueryHandlers();
        }

        private void SetupDomainClasses()
        {
            DomainClasses = Classes().That().ResideInAssembly(".*.Domain", true)
                .And().AreNot(DomainContractClasses)
                .And().AreNot(DatabaseMigrations)
                .And().AreNot(".*ProcessedByFody", true)
                .As("Domain Classes");
        }

        private void SetupOperationHandlers()
        {
            DomainOperationHandlers = Classes().That().Are(DomainClasses)
                .And().ImplementInterface("MediatR.IRequestHandler`2")
                .As("Domain Operation Handlers");
        }
        
        private void SetupCommandHandlers()
        {
            CommandHandlers = Classes().That().Are(DomainOperationHandlers)
                .And().HaveNameEndingWith("CommandHandler")
                .As("Command Handlers");
        }
        
        private void SetupQueryHandlers()
        {
            QueryHandlers = Classes().That().Are(DomainOperationHandlers)
                .And().HaveNameEndingWith("QueryHandler")
                .As("Query Handlers");
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