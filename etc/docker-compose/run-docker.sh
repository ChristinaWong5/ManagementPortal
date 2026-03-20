#!/bin/bash

if [[ ! -d certs ]]
then
    mkdir certs
    cd certs/
    if [[ ! -f localhost.pfx ]]
    then
        dotnet dev-certs https -v -ep localhost.pfx -p ede7bd7c-8c35-4148-adcb-9f3fd05c70ae -t
    fi
    cd ../
fi

docker-compose up -d
