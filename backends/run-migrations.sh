#!/bin/bash
set -e

id=$(docker ps --no-trunc | grep aux | cut -d ' ' -f 1)
docker exec -it "$id" /app/migrate-all.sh
docker kill "$id"
