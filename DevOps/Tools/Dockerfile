FROM alpine:latest
RUN apk update
RUN apk add git
RUN git config --global --add safe.directory /repo
WORKDIR /repo