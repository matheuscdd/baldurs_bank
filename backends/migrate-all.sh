#!/bin/bash
set -e

echo 'Init migrations...'

migrate() {
    /app/.tools/dotnet-ef database update --project src/Repository --startup-project src/Api
}


cd /app/user && migrate
cd /app/account && migrate
cd /app/transaction && migrate

echo 'Migrations completed successfully!'