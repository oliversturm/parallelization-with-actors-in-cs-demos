#!/bin/bash

docker run --rm -it --volume .:/src -w /src oliversturm/dotnet-demo:1 bash -c "tmux new-session -d -s demo 'dotnet run --project HelloServer'; tmux split-window -h -t demo 'dotnet run --project HelloClient'; tmux attach-session -t demo"
