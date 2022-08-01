FROM alpine:3.14.7
RUN apk update && apk upgrade
RUN apk --no-cache add git=2.32.3-r0 \
    sqlite=3.35.5-r0 \
    && git config --global --add safe.directory /repo