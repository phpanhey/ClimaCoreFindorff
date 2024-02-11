#!/bin/bash

#cross compile for linux
dotnet publish -c Release -r linux-x64 --self-contained true

# Load environment variables from .env file
source .env

# Copy the ClimaCoreFindorff file to the destination server using scp
scp ./bin/Release/net8.0/linux-x64/publish/* $REMOTE_SERVER_USER@$REMOTE_SERVER_IP:$REMOTE_SERVER_DIR