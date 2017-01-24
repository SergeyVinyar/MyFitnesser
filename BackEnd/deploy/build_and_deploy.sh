#!/bin/bash
eval "$(docker-machine env myfitnesser)"

./build.sh

docker rm -f $(docker ps -a -q)

docker network create -d bridge private-net
docker run --name=db --network=private-net --restart=always -v /db:/db -e "POSTGRES_USER=$DB_USER" -e "POSTGRES_PASSWORD=$DB_PASSWORD" -e "PGDATA=/db" -e "POSTGRES_DB=myfitnesser" --log-opt max-size=10m --log-opt max-file=10  -d postgres:9.6-alpine
docker run --name=web --network=private-net --restart=always -p 80:8080 -e "DB_HOST=db" -e "DB_PORT=5432" -e "DB_USER=$DB_USER" -e "DB_PASSWORD=$DB_PASSWORD" -e "DB_DATABASE=myfitnesser" --log-opt max-size=10m --log-opt max-file=10 -d myfitnesser-web
