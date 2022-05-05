#! /bin/bash
./build_n_run_dev_dependencies.sh
export GIT_COMMIT_HASH=$(git rev-parse --short HEAD)
docker-compose -f docker-compose.yml build && docker-compose -f docker-compose.yml up -d
docker logs -f conduit-api
docker-compose -f docker-compose.yml down
