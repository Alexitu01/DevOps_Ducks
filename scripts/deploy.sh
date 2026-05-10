#!/bin/bash

set -e

CURRENT=$(cat ../current_color)

if [ "$CURRENT" = "blue" ]; then
    NEW_VERSION="green"
else
    NEW_VERSION="blue"
fi

if [ "$NEW_VERSION" = "blue" ]; then
    PORTS=(8081 8082)
else
    PORTS=(8083 8084)
fi

docker compose pull ${NEW_VERSION}server1 ${NEW_VERSION}server2
docker compose up -d --build ${NEW_VERSION}server1 ${NEW_VERSION}server2 --remove-orphans

echo "Waiting for healthcheck..."
sleep 5
for PORT in "${PORTS[@]}"; do
    curl -sf http://127.0.0.1:$PORT || { echo "Health check failed on port $PORT"; exit 1; }
done

echo "proxy_pass http://${NEW_VERSION};" | sudo tee /etc/nginx/active_upstream.conf > /dev/null
sudo nginx -t && sudo systemctl reload nginx

bash ../change_color.sh