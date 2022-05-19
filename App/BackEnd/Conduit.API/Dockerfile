FROM mcr.microsoft.com/dotnet/sdk:5.0 AS base
WORKDIR /app
COPY *.sln ./
COPY */*.*proj ./
RUN dotnet sln list | \
      tail -n +3 | \
      xargs -I {} sh -c \
        'target="{}"; dir="${target%/*}"; file="${target##*/}"; mkdir -p -- "$dir"; mv -- "$file" "$target"'
#make sure your .dockerignore ignores **/bin and **/obj or dotnet build can fail if the runtime identifier of your host machine and your Docker image are different
RUN dotnet restore Conduit.API --runtime linux-musl-x64

COPY . .

FROM base AS tests
RUN dotnet build -c Release -o output
RUN echo '#!/bin/sh\ndotnet test --no-restore --no-build -c Release -o output' > run-tests.sh && chmod +x run-tests.sh 

FROM base AS publish
ARG ENABLE_TREESHAKING=true

RUN dotnet publish Conduit.API -c Release -o output \
  --no-restore \
  --runtime linux-musl-x64 \
  --self-contained true \
  /p:PublishTrimmed=${ENABLE_TREESHAKING} \
  /p:TrimMode=Link \
  /p:PublishSingleFile=true
  

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine AS final
WORKDIR /app
COPY --from=publish /app/output .
ENTRYPOINT ["./Conduit.API"]
