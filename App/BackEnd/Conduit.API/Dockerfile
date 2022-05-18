ARG minified_image=true

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app
COPY . .

FROM base AS tests
RUN dotnet build -c Release -o output
RUN echo '#!/bin/sh\ndotnet test --no-restore --no-build -c Release -o output' > run-tests.sh && chmod +x run-tests.sh 

FROM base AS publish

RUN dotnet publish Conduit.API -c Release -o output \
  --runtime linux-musl-x64 \
  --self-contained true \
  /p:PublishTrimmed=${minified_image} \
  /p:PublishSingleFile=true
  

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine AS final
WORKDIR /app
COPY --from=publish /app/output .
ENTRYPOINT ["./Conduit.API"]
