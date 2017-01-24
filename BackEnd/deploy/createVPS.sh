#!/bin/bash
docker-machine create --driver digitalocean --digitalocean-access-token $DO_API_KEY myfitnesser
docker-machine ssh myfitnesser mkdir /db