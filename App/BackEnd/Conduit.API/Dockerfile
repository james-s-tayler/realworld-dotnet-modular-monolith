FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS base
WORKDIR /app
#dotnet restore only needs .sln and .csproj files in order to run, since these change infrequently and dotnet restore takes a long time to run
#it's good to do COPY . . after this step so that dotnet restore can be effectively cached to speed up the docker image build
#another way to accomplish this is to use:
#RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages\
#    dotnet build --configuration Release --no-restore
#which is a more modern, portable solution, but not sure if it works when the host machine and docker image are building for different runtime identifiers
#hence going with the more traditional approach 
COPY Conduit.API.sln ./
COPY Directory.Build.props ./
COPY */*.csproj ./
RUN dotnet sln list | \
      grep ".csproj" | \
      xargs -I {} sh -c \
        'target="{}"; dir="${target%/*}"; file="${target##*/}"; mkdir -p -- "$dir"; mv -- "$file" "$target"'

#make sure your .dockerignore ignores **/bin and **/obj or dotnet build can fail 
#if the runtime identifier of your host machine and your Docker image are different
#saying that it can't find the DLLs and you need to do a NuGet restore or might have done a partial one
#NB: linux-musl-x64 is the runtime identifier needed for the alpine images
RUN dotnet restore Conduit.API --runtime linux-musl-x64

FROM base AS tests
COPY . .
RUN dotnet build Conduit.API -c Release -o output \
  --no-restore \
  --runtime linux-musl-x64
RUN echo '#!/bin/sh\ndotnet test --no-restore --no-build -c Release -o output' > run-tests.sh && chmod +x run-tests.sh 

FROM base AS publish
#programmatically strip out the tests from the sln before building
RUN for test_project in `dotnet sln Conduit.API.sln list | grep ".csproj" | grep "Tests"`; do dotnet sln Conduit.API.sln remove ${test_project}; done;

COPY . .

#TrimMode=CopyUsed is a bit safer than than TrimMode=Link, but file size is larger
#either way static analyzers may accidentally trim code being used via reflection if suitable compile time attributes aren't used where needed
#use at your own risk and ensure sufficient test coverage is in place
ARG ENABLE_TREESHAKING=false
ARG MODEOF_TREESHAKING=CopyUsed

RUN dotnet publish Conduit.API -c Release -o output \
  --no-restore \
  --runtime linux-musl-x64 \
  --self-contained true \
  /p:PublishTrimmed=${ENABLE_TREESHAKING} \
  /p:TrimMode=${MODEOF_TREESHAKING} \
  /p:PublishSingleFile=true
  

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine AS final
WORKDIR /app
COPY --from=publish /app/output .
ENTRYPOINT ["./Conduit.API"]
