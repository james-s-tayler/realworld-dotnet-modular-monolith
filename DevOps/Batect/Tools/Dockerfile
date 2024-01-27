FROM alpine:3.19.1
RUN apk update && apk upgrade
RUN apk --no-cache add  \
    git=2.36.6-r0 \
    sqlite=3.40.1-r0 \
    && git config --global --add safe.directory /repo