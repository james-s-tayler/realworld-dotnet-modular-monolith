FROM alpine:3.16.1
RUN apk update && apk upgrade
RUN apk --no-cache add  \
    git=2.36.2-r0 \
    sqlite=3.38.5-r0 \
    && git config --global --add safe.directory /repo