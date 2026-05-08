#!/bin/bash

VM1="root@$VM1_HOST"
VM2="root@$VM2_HOST"

ssh $VM1 "bash deploy.sh" &&
ssh $VM2 "bash deploy.sh"
wait

echo "Update done"