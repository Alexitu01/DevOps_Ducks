#!/bin/bash

VM1="root@$VM1_HOST"
VM2="root@$VM2_HOST"

ssh $VM1 "bash scripts/deploy.sh" &
ssh $VM2 "bash scripts/deploy.sh" &
wait

echo "Update done"