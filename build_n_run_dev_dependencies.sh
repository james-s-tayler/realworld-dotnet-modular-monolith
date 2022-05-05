#! /bin/bash
docker-compose -f docker-compose.dev-dependencies.yml build && docker-compose -f docker-compose.dev-dependencies.yml up -d --remove-orphans
