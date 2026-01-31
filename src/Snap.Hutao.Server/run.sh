#!/bin/bash

imageName=snapserverimg
containerName=snapserver
http_port=5076
https_port=7076
version=1.2

oldContainer=`docker ps -a| grep ${containerName} | head -1|awk '{print $1}' `
echo Delete old container...
docker rm  $oldContainer -f
echo Delete success

echo start build...
docker build -t $imageName:$version -f Dockerfile .

echo "HTTP port is $http_port, HTTPS port is $https_port"
docker run -d \
    -p $http_port:80 \
    -p $https_port:443 \
    --name="$containerName-$version" \
    $imageName:$version 

echo "Server deployed successfully!"
echo "Access URLs:"
echo "  HTTP:  http://localhost:$http_port"
echo "  HTTPS: https://localhost:$https_port"
echo "  API Documentation: http://localhost:$http_port/doc"
echo "  Static Pages:"
echo "    - Home: http://localhost:$http_port/"
echo "    - Redeem: http://localhost:$http_port/redeem.html"
