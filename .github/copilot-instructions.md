# RealWorld .NET Modular Monolith

**ALWAYS follow these instructions first and fallback to additional search and context gathering only if the information in the instructions is incomplete or found to be in error.**

This is a .NET 6 modular monolith implementation of the RealWorld API using Clean/Hexagonal architecture. The application is containerized and uses Batect for all build/test/run operations.

## Working Effectively

### Bootstrap and Build
**CRITICAL**: Always use these EXACT commands with specified timeouts. NEVER CANCEL builds or tests - they may take 45+ minutes.

#### Primary Approach (Batect)
```bash
# List all available tasks
./batect --list-tasks

# Build the application - takes 20-30 minutes. NEVER CANCEL. Set timeout to 60+ minutes.
./batect app:build

# Run unit tests - takes 10-15 minutes. NEVER CANCEL. Set timeout to 30+ minutes.
./batect app:test:unit

# Run API tests - takes 5-10 minutes. NEVER CANCEL. Set timeout to 20+ minutes.
./batect app:test:api

# Run the application
./batect app:run
```

#### Fallback Approach (Network Issues)
If Batect download fails due to network connectivity (common in restricted environments):

```bash
# Ensure .NET 8 SDK is available (fallback compatibility)
dotnet --version

# Restore packages (required for build) - may fail with NU1301 network errors
dotnet restore App/BackEnd/Conduit.API.sln -p:NuGetAudit=false

# Build solution - takes 15-20 minutes. NEVER CANCEL. Set timeout to 45+ minutes.
cd App/BackEnd && dotnet build Conduit.API.sln --no-restore

# Run tests (requires .NET 6 runtime - may fail with .NET 8 only)
dotnet test App/BackEnd/Conduit.API.sln --no-build -p:NuGetAudit=false
```

#### Docker Approach (Network Permitting)
```bash
# Build with Docker - takes 25-35 minutes. NEVER CANCEL. Set timeout to 60+ minutes.
cd App/BackEnd && docker build --target publish .

# Run tests with Docker
cd App/BackEnd && docker build --target tests .
```

### Known Issues and Workarounds

- **Network Restrictions**: Batect download and NuGet restore may fail in sandboxed environments
- **Version Compatibility**: Project targets .NET 6 but can build with .NET 8 SDK using fallback approach
- **Security Vulnerabilities**: Some packages have known vulnerabilities, use `-p:NuGetAudit=false` to bypass
- **Architecture Tests**: Separate from unit tests, run with: `./batect lint:dotnet:architecture`

## Validation

### Manual Testing Scenarios
After making changes, ALWAYS test these complete scenarios:

1. **API Health Check**: 
   - Start application with `./batect app:run`
   - Verify API responds at `http://localhost:5100/api/health` (if available)
   - Check Swagger UI at `http://localhost:5100/swagger`

2. **Database Operations**:
   - Run `./batect app:db:scrub` to reset databases
   - Verify SQLite databases exist in `./sqlite/` directory

3. **End-to-End API Flow**:
   - Use Postman collection in `DevOps/Batect/ApiTests/Conduit.postman_collection.json`
   - Test user registration, login, article creation

### Pre-Commit Validation
ALWAYS run these before committing:
```bash
# Format check - NEVER CANCEL. Set timeout to 10+ minutes.
./batect lint:dotnet:format

# Architecture tests - NEVER CANCEL. Set timeout to 15+ minutes.
./batect lint:dotnet:architecture

# All linting - NEVER CANCEL. Set timeout to 20+ minutes.
./batect lint
```

## Architecture and Navigation

### Key Projects Structure
```
App/BackEnd/
├── Conduit.API/                    # Main API project (entry point)
├── App.Core/                       # Shared framework code
├── App.Core.Testing/               # Testing utilities
├── App.Users.Domain/               # Users domain (internal)
├── App.Users.Domain.Contracts/     # Users domain contracts (public)
├── App.Users.Domain.Tests.Unit/    # Users domain tests
├── App.Content.Domain/             # Content domain (internal)
├── App.Content.Domain.Contracts/   # Content domain contracts (public)
├── App.Content.Domain.Tests.Unit/  # Content domain tests
├── App.Feed.Domain/                # Feed domain (internal)
├── App.Feed.Domain.Contracts/      # Feed domain contracts (public)
├── App.Feed.Domain.Tests.Unit/     # Feed domain tests
└── App.FitnessFunctions.ArchitectureTests/ # Architecture validation
```

### Domain Boundaries
- **Strict Isolation**: Each domain has its own database (SQLite files in `./sqlite/`)
- **Public Contracts**: Only `*.Domain.Contracts` projects are public interfaces
- **Internal Implementation**: All `*.Domain` projects are marked internal
- **Event Communication**: Domains communicate via MediatR notifications

### Code Generation
Generate new modules and operations using provided scripts:
```bash
# Generate new domain module
cd App/BackEnd && ./generate_module.sh ModuleName

# Generate new command
cd App/BackEnd && ./generate_operation_command.sh ModuleName OperationName

# Generate new query  
cd App/BackEnd && ./generate_operation_query.sh ModuleName OperationName
```

## Common Tasks

### Adding New Features
1. Generate scaffolding using scripts above
2. Implement contracts in `*.Domain.Contracts` project
3. Implement handlers/validators in `*.Domain` project
4. Add unit tests in `*.Domain.Tests.Unit` project
5. Update architecture tests if needed
6. Always test the complete user workflow

### Debugging Issues
- Check `batect.yml` for container configurations
- SQLite databases are in `./sqlite/` directory
- Logs are structured using Serilog
- OpenTelemetry tracing available via Jaeger
- Architecture violations caught by ArchUnit.NET tests

### CI/CD Pipeline
The GitHub Actions workflows in `.github/workflows/` mirror Batect tasks:
- Pull request builds run: `app:test:unit` and `app:test:api`
- All linting tasks must pass
- Deployment pipeline runs: `app:build`

## Time Expectations

| Operation | Expected Time | Timeout Setting |
|-----------|--------------|-----------------|
| `./batect app:build` | 20-30 minutes | 60+ minutes |
| `./batect app:test:unit` | 10-15 minutes | 30+ minutes |
| `./batect app:test:api` | 5-10 minutes | 20+ minutes |
| `./batect lint` | 15-20 minutes | 45+ minutes |
| `dotnet build` (fallback) | 15-20 minutes | 45+ minutes |

**CRITICAL**: NEVER CANCEL long-running commands. The build system uses Docker containers with NuGet package restoration which can take significant time on first run.