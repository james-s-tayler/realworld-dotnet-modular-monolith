FROM alpine:3.16.2
RUN apk update && apk upgrade
RUN apk --no-cache add  \
    git \
    sqlite \
    && git config --global --add safe.directory /repo