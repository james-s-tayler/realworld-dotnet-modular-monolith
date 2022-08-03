FROM mcr.microsoft.com/dotnet/sdk:6.0.302 AS base
WORKDIR /app
COPY . .

FROM base AS lint
RUN dotnet format Conduit.API.sln --verify-no-changes

FROM base AS tests
RUN printf "#! /bin/sh \n \
dotnet test Conduit.API.sln" \
    > /app/run-tests.sh && chmod +x /app/run-tests.sh

FROM base AS publish
#TrimMode=CopyUsed is a bit safer than than TrimMode=Link, but file size is larger
#either way static analyzers may accidentally trim code being used via reflection if suitable compile time attributes aren't used where needed
#use at your own risk and ensure sufficient test coverage is in place
ARG ENABLE_TREESHAKING=false
ARG MODEOF_TREESHAKING=CopyUsed

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish Conduit.API -c Release -o output \
    --self-contained true \
    --runtime linux-x64 \
    /p:PublishTrimmed=${ENABLE_TREESHAKING} \
    /p:TrimMode=${MODEOF_TREESHAKING} \
    /p:PublishSingleFile=true
  
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0.7 AS final
WORKDIR /app
COPY --from=publish /app/output .
ENTRYPOINT ["./Conduit.API"]
