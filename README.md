# RealWorld Backend - .NET Modular Monolith

This is an implementation of the [RealWorld](https://github.com/gothinkster/realworld) API. It's written in .NET 9 and is a "[modular monolith](https://www.youtube.com/watch?v=5OjqD-ow8GE&ab_channel=GOTOConferences)".

## Building / Running
Everything is done via [Batect](https://batect.dev/). Just use `./batect --list-tasks` to see everything you can do. You can build the app, run the app, run the unit tests, run the postman tests, and perform various linting and database operations. 

All of these exact same `batect` tasks are run in `GitHub Actions` in order to implement the `CI/CD` pipeline.

## Tech Stack

 - Backend
   - .NET 9
   - MediatR
   - Dapper
   - Sqlite
   - Serilog
   - ArchUnit.NET
   - FluentMigrator
   - FluentValidation
   - xUnit
 - DevOps
   - Docker
   - Batect
   - GitHub Actions
   - Renovate

## Learning Goals

 - Have a go at writing a modular monolith!
 - Have a go at Clean / Hexagonal architecture
 - Implement a full Continuous Deployment pipeline
 - Learn ArchUnit.NET
 - Learn GitHub Actions
 - Learn Batect
 - Learn Renovate
 - Learn .NET Templating

## Architecture

The logic is split into 3 separate domains:

 - Users Domain
 - Content Domain
 - Feed Domain

Each domain has 3 projects associated with it:

 - `App.${ModuleName}.Domain`
 - `App.${ModuleName}.Domain.Contracts`
 - `App.${ModuleName}.Domain.Tests`

Each `App.${ModuleName}.Domain` project contains a `Setup/Module/${ModuleName}Module.cs` which subclasses `AbstractModule` which implements [IHostingStartup](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/platform-specific-configuration?view=aspnetcore-6.0) 
and provides a framework for loading all `MediatR` use cases in the module as well as setting up an `IDbConnection` to that modules database. Each module has it's own database which only it can access. Domain operations are split into commands, and queries and implemented via `MediatR`.
Everything inside the domain project itself is marked as `internal` so that nothing else can access its logic or database directly. The API project contains a direct project reference to each module and is loaded by including the assembly name in the comma seperated list in `HostingStartupAssembliesKey`.

Each `App.${ModuleName}.Domain.Contracts` project contains all of the contracts of the domain operations. Everything in the contracts project is marked as `public`.
This represents the strict boundary through which other modules can interact with the domain.

Each `App.${ModuleName}.Domain.Tests` project contains one test class per domain operation. All testing is done at the contracts boundary via `MediatR`.

[ArchUnit.NET](https://archunitnet.readthedocs.io/en/latest/) is used to enforce that each module conform to the conventions a module is expected to follow
in terms of folder structures, namespaces, access modifiers, and naming conventions.

Domain modules can listen for events that happen in other domain modules and perform any processing accordingly.
Each `${OperationName}CommandResult` implements `INotification` which a `MediatR` pipeline behavior uses to publish these events.
Domain modules interested in a particular operation simply implement an `INotificationHandler<${OperationName}CommandResult>` under
`Infrastructure/EventListeners/${OperationName}CommandResultListener.cs`

A set of .NET templates are provided for quickly creating both new domain modules (`AppTemplates/Module/generate_module.sh`) and new domain operations (`AppTemplates/Operation/generate_operation_command.sh` / `AppTemplates/Operation/generate_operation_query.sh`).