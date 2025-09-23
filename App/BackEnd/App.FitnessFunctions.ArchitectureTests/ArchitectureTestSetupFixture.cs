using System;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using App.Core.Modules;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using FluentMigrator;
using MediatR;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace App.FitnessFunctions.ArchitectureTests
{
    public class ArchitectureTestSetupFixture : IDisposable
    {
        public Architecture Architecture { get; }
        public IObjectProvider<Class> DomainContracts { get; private set; }
        public IObjectProvider<Class> DomainOperations { get; private set; }
        public IObjectProvider<Class> DomainOperationHandlers { get; private set; }
        public IObjectProvider<Class> DomainModules { get; private set; }
        public IObjectProvider<Class> DomainClasses { get; private set; }
        public IObjectProvider<Class> Repositories { get; private set; }
        public IObjectProvider<Class> Entities { get; private set; }
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
            DatabaseMigrations = Classes().That()
                .AreAssignableTo(typeof(Migration))
                .And().AreNot(Classes().That().HaveNameMatching(".*ProcessedByFody"))
                .As("Database Migrations");
        }

        private void SetupDomain()
        {
            SetupDomainClasses();
            SetupDomainModules();
            SetupEntities();
            SetupOperationHandlers();
            SetupCommandHandlers();
            SetupQueryHandlers();
            SetupRepositories();
        }

        private void SetupRepositories()
        {
            Repositories = Classes().That().Are(DomainClasses)
                .And().DependOnAny(typeof(DbConnection))
                .As("Repositories");
        }

        private void SetupDomainClasses()
        {
            DomainClasses = Classes().That().ResideInAssemblyMatching("App.*.Domain")
                .And().AreNot(DomainContracts)
                .And().AreNot(DatabaseMigrations)
                .And().AreNot(Classes().That().HaveNameMatching(".*ProcessedByFody"))
                .As("Domain Classes");
        }

        private void SetupDomainModules()
        {
            DomainModules = Classes().That().Are(DomainClasses)
                .And()
                .AreAssignableTo(typeof(AbstractModule))
                .As("Domain Modules");
        }

        private void SetupEntities()
        {
            Entities = Classes().That().Are(DomainClasses)
                .And().ResideInNamespaceMatching("App.*.Domain.Entities.*")
                .As("Entities");
        }

        private void SetupOperationHandlers()
        {
            DomainOperationHandlers = Classes().That().Are(DomainClasses)
                .And().ImplementInterface(typeof(IRequestHandler<,>))
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
            DomainContracts = Classes().That().ResideInAssemblyMatching("App.*.Domain.Contracts.*")
                .And().AreNot(Classes().That().HaveNameMatching(".*ProcessedByFody"))
                .As("Domain Contracts");
        }

        private void SetupOperations()
        {
            DomainOperations = Classes().That().Are(DomainContracts)
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

        public System.Reflection.Assembly[] GetSolutionAssemblies()
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