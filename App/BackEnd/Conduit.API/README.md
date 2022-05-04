# Conduit.API - ASP.NET Core 5.0 Server

Conduit API

## Run

Linux/OS X:

```
sh build.sh
```

Windows:

```
build.bat
```
## Run in Docker

```
cd src/Conduit.API
docker build -t conduit.api .
docker run -p 5000:8080 conduit.api
```
