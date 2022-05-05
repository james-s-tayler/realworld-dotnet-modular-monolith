FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore -nowarn:msb3202,nu1503
RUN dotnet build --no-restore -c Release -o output

FROM build AS tests
RUN echo '#!/bin/sh\ndotnet test --no-restore --no-build -c Release -o output' > run-tests.sh && chmod +x run-tests.sh 

FROM build AS publish
RUN dotnet publish --no-restore -c Release -o output

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS final
WORKDIR /app
COPY --from=publish /app/output .
ENTRYPOINT ["dotnet", "Conduit.API.dll"]
