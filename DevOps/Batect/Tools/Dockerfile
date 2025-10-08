FROM alpine:3.22.2
RUN apk update && apk upgrade
RUN apk --no-cache add  \
    git=2.49.1-r0 \
    sqlite=3.49.2-r1 \
    && git config --global --add safe.directory /repo