#!/bin/bash
set -e

echo 'Init migrations...'

cd /app/user
/app/.tools/dotnet-ef database update --project src/Repository --startup-project src/Api

cd /app/account
/app/.tools/dotnet-ef database update --project src/Repository --startup-project src/Api

echo 'Migrations completed successfully!'