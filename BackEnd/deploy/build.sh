#!/bin/bash
cd ..
mvn package
if [ $? -eq 0 ]; then

cd deploy
mkdir -p web
rm -r web/*
cp Dockerfile_web web/Dockerfile
mkdir -p ./web/public_html
cp ../server.jar ./web/
cp -r ../public_html ./web/

cd web
docker build -t myfitnesser-web .
fi