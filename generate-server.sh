#! /bin/bash

docker run --rm \
    -v $PWD:/local openapitools/openapi-generator-cli generate \
    -i /local/openapi.yaml \
    -g aspnetcore \
    --additional-properties=aspnetCoreVersion=5.0,operationIsAsync=true,operationResultTask=true,useNewtonsoft=false \
    -o /local/App/BackEnd/Conduit.API \
    --package-name Conduit.API

