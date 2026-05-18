#!/bin/bash

CURRENT=$(cat ./current_color)

if [[ "$CURRENT" == "blue" ]]; then 
        NEXT="green"
else
        NEXT="blue"
fi

echo "$NEXT" > ./current_color

echo "$NEXT"
